using Microsoft.EntityFrameworkCore;
using CorpEntities;
using CorpApi;
using static CorpApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace CorpApi
{
    /// <summary>
    /// API /products
    /// </summary>
    public static class ProductsApi
    {
        public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            //Получить таблицу товаров
            api.MapGet("/", [Authorize] async (PharmacyContext db) =>
            {
                var products = await db.Products
                    .Include(p => p.Category) 
                    .OrderBy(p => p.ProductId) 
                    .Select(p => new
                    {
                        p.ProductId,           
                        p.Name,                
                        Category = p.Category.CategoryName, 
                        p.Quantity,           
                        p.Price,               
                        p.Supplier            
                    })
                    .ToListAsync();

                return Results.Ok(new
                {
                    Message = "Товары", 
                    Data = products
                });
            });

            //Получить товар по ID
            api.MapGet("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var product = await db.Products
                    .Include(p => p.Category)
                    .Where(p => p.ProductId == id)
                    .Select(p => new
                    {
                        p.ProductId,         
                        p.Name,               
                        Category = p.Category.CategoryName, 
                        p.Quantity,           
                        p.Price,              
                        p.Supplier           
                    })
                    .FirstOrDefaultAsync();

                return product is not null ? Results.Ok(product) : Results.NotFound($"Товар с ID {id} не найден.");
            });

            //Получить товар по наименованию
            api.MapGet("/name/{name}", [Authorize] async (string name, PharmacyContext db) =>
            {
                var product = await db.Products
                    .Include(p => p.Category) 
                    .Where(p => p.Name.ToLower() == name.ToLower()) //Ищем по наименованию без учета регистра
                    .Select(p => new
                    {
                        p.ProductId,          
                        p.Name,               
                        Category = p.Category.CategoryName, 
                        p.Quantity,            
                        p.Price,               
                        p.Supplier             
                    })
                    .FirstOrDefaultAsync();

                return product is not null ? Results.Ok(product) : Results.NotFound("Товар не найден.");
            });


            //Добавить новый товар
            api.MapPost("/", [Authorize] async (ProductDto productDto, PharmacyContext db) =>
            {
                var category = await db.Categories.FindAsync(productDto.CategoryId);

                if (category == null)
                {
                    return Results.NotFound(new { message = "Категория не найдена" });
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Category = category, 
                    Quantity = productDto.Quantity,
                    Price = productDto.Price,
                    Supplier = productDto.Supplier
                };

                db.Products.Add(product);
                await db.SaveChangesAsync();

                return Results.Created($"/pharmacy/products/{product.ProductId}", product);
            });


            //Обновить информацию о товаре
            api.MapPut("/{id}", [Authorize] async (int id, UpdateProductDto productDto, PharmacyContext db) =>
            {
                var existingProduct = await db.Products.FindAsync(id);
                if (existingProduct is null)
                    return Results.NotFound(new { message = "Товар с указанным ID не найден." });

                if (productDto.CategoryId.HasValue)
                {
                    var category = await db.Categories.FindAsync(productDto.CategoryId.Value);
                    if (category is null)
                        return Results.NotFound(new { message = "Категория с указанным ID не найдена." });

                    existingProduct.CategoryId = productDto.CategoryId.Value;
                }

                if (productDto.Name != null)
                    existingProduct.Name = productDto.Name;

                if (productDto.Quantity.HasValue)
                    existingProduct.Quantity = productDto.Quantity.Value;

                if (productDto.Price.HasValue)
                    existingProduct.Price = productDto.Price.Value;

                if (productDto.Supplier != null)
                    existingProduct.Supplier = productDto.Supplier;

                await db.SaveChangesAsync();
                return Results.Ok(existingProduct);
            });

            // Удалить продукт
            api.MapDelete("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null)
                {
                    return Results.NotFound(new { message = $"Товар с ID {id} не найден." });
                }

                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });


            return api;
        }
    }
}
