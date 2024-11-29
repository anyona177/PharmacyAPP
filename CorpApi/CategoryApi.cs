using Microsoft.EntityFrameworkCore;
using CorpEntities;
using Microsoft.AspNetCore.Authorization;

namespace CorpApi
{
    /// <summary>
    /// API /categories
    /// </summary>
    public static class CategoriesApi
    {
        public static RouteGroupBuilder MapCategoriesApi(this RouteGroupBuilder api)
        {
            //Получить таблицу с категориями
            api.RequireAuthorization();
            api.MapGet("/", [Authorize] async (PharmacyContext db) =>
            {
                var categories = await db.Categories
                                         .OrderBy(c => c.CategoryId)
                                         .Select(c => new
                                         {
                                             c.CategoryId,
                                             c.CategoryName
                                         })
                                         .ToListAsync();

                return Results.Ok(new
                {
                    Message = "Категории",
                    Data = categories
                });
            });

            //Получить категорию по ID
            api.MapGet("/{id}", [Authorize] async (int id, PharmacyContext db) =>
            {
                var category = await db.Categories
                                       .Where(c => c.CategoryId == id)
                                       .Select(c => new
                                       {
                                           c.CategoryId,
                                           c.CategoryName
                                       })
                                       .FirstOrDefaultAsync();

                return category is not null
                    ? Results.Ok(category)
                    : Results.NotFound($"Категория с ID {id} не найдена.");
            });

            //Поиск категории по названию
            api.MapGet("/name/{name}",[Authorize] async (string name, PharmacyContext db) =>
            {
                var categories = await db.Categories
                                         .Where(c => c.CategoryName.ToLower().Contains(name.ToLower()))
                                         .Select(c => new
                                         {
                                             c.CategoryId,
                                             c.CategoryName
                                         })
                                         .ToListAsync();

                return categories.Any()
                    ? Results.Ok(categories)
                    : Results.NotFound("Категории не найдены");
            });


            //Добавить новую категорию
            api.MapPost("/", [Authorize(Roles = "Администратор")] async (Category category, PharmacyContext db) =>
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return Results.Created($"/categories/{category.CategoryId}", category);
            });

            //Обновить информацию о категории
            api.MapPut("/{id}", [Authorize(Roles = "Администратор")] async (int id, Category category, PharmacyContext db) =>
            {
                var existingCategory = await db.Categories.FindAsync(id);
                if (existingCategory is null) return Results.NotFound($"Категория с ID {id} не найдена.");

                existingCategory.CategoryName = category.CategoryName;

                await db.SaveChangesAsync();
                return Results.Ok(existingCategory);
            });

            //Удалить категорию
            api.MapDelete("/{id}", [Authorize(Roles = "Администратор")] async (int id, PharmacyContext db) =>
            {
                var category = await db.Categories.FindAsync(id);
                if (category is null) return Results.NotFound($"Категория с ID {id} не найдена.");

                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return api;
        }
    }
}
