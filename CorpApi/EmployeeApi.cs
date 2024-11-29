using Microsoft.EntityFrameworkCore;
using CorpEntities;
using static CorpApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace CorpApi
{
    /// <summary>
    /// API /employees
    /// </summary>
    public static class EmployeesApi
    {
        public static RouteGroupBuilder MapEmployeesApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            //Получить таблицу сотрудников
            api.MapGet("/", [Authorize] async (PharmacyContext db) =>
            {
                var employees = await db.Employees
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.Surname ,   
                        e.Name,        
                        Patronymic = e.Patronymic ?? string.Empty, 
                        e.Post,
                        e.Phone
                    })
                    .ToListAsync();

                return Results.Ok(new
                {
                    Message = "Сотрудники",
                    Data = employees
                });
            });

            //Получить сотрудника по ID
            api.MapGet("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var employee = await db.Employees
                    .Where(e => e.EmployeeId == id)
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.Surname,
                        e.Name,
                        Patronymic = e.Patronymic ?? string.Empty,
                        e.Post,
                        e.Phone
                    })
                    .FirstOrDefaultAsync();

                return employee is not null
                    ? Results.Ok(employee)
                    : Results.NotFound("Сотрудник не найден");
            });

            // Поиск сотрудника по имени, фамилии или отчеству
            api.MapGet("/name/{name}", [Authorize] async (string name, PharmacyContext db) =>
            {
                var employees = await db.Employees
                    .Where(e =>
                        (e.Name != null && e.Name.ToLower().Contains(name.ToLower())) ||
                        (e.Surname != null && e.Surname.ToLower().Contains(name.ToLower())) ||
                        (e.Patronymic != null && e.Patronymic.ToLower().Contains(name.ToLower())))
                    .OrderBy(e => e.EmployeeId)
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.Surname,
                        e.Name,
                        Patronymic = e.Patronymic ?? string.Empty,
                        e.Post,
                        e.Phone
                    })
                    .ToListAsync();

                return employees.Any()
                    ? Results.Ok(employees)
                    : Results.NotFound("Сотрудники не найдены");
            });

            //Добавить нового сотрудника
            api.MapPost("/", async (Employee employee, PharmacyContext db) =>
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return Results.Created($"/pharmacy/employees/{employee.EmployeeId}", employee);
            });

            //Обновление данных о сотруднике
            api.MapPut("/{id}", [Authorize(Roles = "Администратор")] async (int id, UpdateEmployeeDto employeeDto, PharmacyContext db) =>
            {
                var existingEmployee = await db.Employees.FindAsync(id);
                if (existingEmployee is null)
                    return Results.NotFound($"Сотрудник с ID {id} не найден.");

                if (!string.IsNullOrEmpty(employeeDto.Name))
                    existingEmployee.Name = employeeDto.Name;

                if (!string.IsNullOrEmpty(employeeDto.Surname))
                    existingEmployee.Surname = employeeDto.Surname;

                if (!string.IsNullOrEmpty(employeeDto.Patronymic))
                    existingEmployee.Patronymic = employeeDto.Patronymic;

                if (!string.IsNullOrEmpty(employeeDto.Post))
                    existingEmployee.Post = employeeDto.Post;

                if (!string.IsNullOrEmpty(employeeDto.Phone))
                    existingEmployee.Phone = employeeDto.Phone;

                await db.SaveChangesAsync();
                return Results.Ok(existingEmployee);
            });

            // Удалить сотрудника
            api.MapDelete("/{id}", [Authorize(Roles = "Администратор")] async (int id, PharmacyContext db) =>
            {
                var employee = await db.Employees.FindAsync(id);
                if (employee is null)
                    return Results.NotFound($"Сотрудник с ID {id} не найден.");

                db.Employees.Remove(employee);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}
