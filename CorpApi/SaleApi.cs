using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using CorpEntities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CorpApi
{
    /// <summary>
    /// API /sales
    /// </summary>
    public static class SalesApi
    {
        public static RouteGroupBuilder MapSalesApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            //Получить таблицу продаж
            api.MapGet("/", [Authorize] async (PharmacyContext db) =>
            {
                var sales = await db.Sales
                    .Include(s => s.Employee)
                    .Select(s => new
                    {
                        s.SaleId,
                        Employee = s.Employee.Surname + " " + s.Employee.Name + " " + s.Employee.Patronymic,
                        SaleDate = s.SaleDate.ToString("dd.MM.yyyy HH:mm:ss"),
                        s.TotalAmount
                    })
                    .OrderBy(s => s.SaleId) 
                    .ToListAsync();

                return Results.Ok(new
                {
                    Message = "Продажи", 
                    Data = sales
                });
            });

            //Получить продажу по ID
            api.MapGet("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var sale = await db.Sales
                    .Include(s => s.Employee)
                    .Where(s => s.SaleId == id)
                    .Select(s => new
                    {
                        s.SaleId,
                        Employee = s.Employee.Surname + " " + s.Employee.Name + " " + s.Employee.Patronymic,
                        SaleDate = s.SaleDate.ToString("dd.MM.yyyy HH:mm:ss"),
                        s.TotalAmount
                    })
                    .FirstOrDefaultAsync();

                return sale is not null
                    ? Results.Ok(sale)
                    : Results.NotFound("Продажа не найдена");
            });

            //Получить продажи по сотруднику
            api.MapGet("/employee/{employeeName}", [Authorize] async (string employeeName, PharmacyContext db) =>
            {
                var sales = await db.Sales
                    .Include(s => s.Employee)
                    .Where(s => (s.Employee.Surname + " " + s.Employee.Name + " " + s.Employee.Patronymic)
                    .ToLower().Contains(employeeName.ToLower())) 
                    .Select(s => new
                    {
                        s.SaleId,
                        Employee = s.Employee.Surname + " " + s.Employee.Name + " " + s.Employee.Patronymic,
                        SaleDate = s.SaleDate.ToString("dd.MM.yyyy HH:mm:ss"),
                        s.TotalAmount
                    })
                    .OrderBy(s => s.SaleId) 
                    .ToListAsync();

                return sales.Any()
                    ? Results.Ok(sales)
                    : Results.NotFound("Продажи не найдены");
            });

            //Добавить продажу
            api.MapPost("/create-sale", [Authorize(Roles = "Фармацевт")] async ([FromBody] Dtos request, PharmacyContext db) =>
            {
                var employee = await db.Employees.FindAsync(request.Sale.EmployeeId);
                if (employee == null)
                {
                    return Results.NotFound("Сотрудник не найден.");
                }

                //Создаем новую продажу и задаем Employee
                var sale = new Sale
                {
                    EmployeeId = request.Sale.EmployeeId,
                    Employee = employee,
                    SaleDate = DateTime.UtcNow, // Устанавливаем текущее время
                    SoldItems = new List<SoldItem>(),
                    TotalAmount = 0
                };

                //Получаем информацию о товарах и добавляем их в продажу
                foreach (var item in request.SoldItems)
                {
                    var product = await db.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        if (product.Quantity < item.Quantity)
                        {
                            return Results.BadRequest($"Недостаточно товара '{product.Name}' на складе.");
                        }

                        //Уменьшаем количество товара на складе
                        product.Quantity -= item.Quantity;

                        var soldItem = new SoldItem
                        {
                            Sale = sale,
                            Product = product,
                            ProductId = product.ProductId,
                            Quantity = item.Quantity,
                            Price = product.Price
                        };

                        sale.TotalAmount += soldItem.Price * soldItem.Quantity;
                        sale.SoldItems.Add(soldItem);
                    }
                    else
                    {
                        return Results.NotFound($"Товар с ID {item.ProductId} не найден.");
                    }
                }

                db.Sales.Add(sale);
                await db.SaveChangesAsync();

                return Results.Created($"/sales/{sale.SaleId}", sale);
            });


            //Удалить продажу
            api.MapDelete("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var sale = await db.Sales.FindAsync(id);
                if (sale is null) return Results.NotFound("Продажа с указанным ID не найдена.");

                db.Sales.Remove(sale);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });


            return api;
        }
    }
}
