using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CorpEntities;

namespace CorpTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            const string connectionString =
                "Host=localhost;Port=5433;Database=appdb;Persist Security Info=True;"
                + "User ID=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<PharmacyContext>();

            using PharmacyContext db = new(
                optionsBuilder.UseNpgsql(connectionString).Options);

            // Удаление данных из таблиц для теста
            db.Categories.RemoveRange(db.Categories.ToList());
            db.Products.RemoveRange(db.Products.ToList());
            db.Sales.RemoveRange(db.Sales.ToList());
            db.SaveChanges();

            // Добавление категорий
            var category1 = new Category { CategoryId = Guid.NewGuid(), CategoryName = "Лекарства" };
            var category2 = new Category { CategoryId = Guid.NewGuid(), CategoryName = "БАДы" };

            db.Categories.AddRange(category1, category2);
            db.SaveChanges();

            // Добавление продуктов
            var product1 = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = "Аспирин",
                Quantity = 100,
                Price = 50.0m,
                Supplier = "ФармГрупп",
                CategoryId = category1.CategoryId, // Внешний ключ на категорию
                Category = category1 // Установка связи с категорией
            };

            var product2 = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = "Иммунал",
                Quantity = 150,
                Price = 350.0m,
                Supplier = "Здоровье",
                CategoryId = category2.CategoryId, // Внешний ключ на категорию
                Category = category2 // Установка связи с категорией
            };

            db.Products.AddRange(product1, product2);
            db.SaveChanges();

            // Добавление продажи
            var employeeId = Guid.NewGuid(); // Замените на реальный идентификатор сотрудника
            var employee = new Employee
            {
                EmployeeId = employeeId,
                Name = "Иван", // Обязательное свойство
                Surname = "Иванов", // Обязательное свойство
                Post = "Фармацевт", // Обязательное свойство
                Phone = "123-456-7890" // Обязательное свойство
            };

            var sale1 = new Sale
            {
                SaleId = Guid.NewGuid(),
                EmployeeId = employeeId, // Внешний ключ на сотрудника
                Employee = employee, // Установка связи с сотрудником
                SaleDate = DateTime.Now
            };

            db.Employees.Add(employee);
            db.Sales.Add(sale1);
            db.SaveChanges();

            // Добавление проданных товаров
            db.SoldItems.Add(new SoldItem
            {
                SoldItemId = Guid.NewGuid(),
                SaleId = sale1.SaleId,
                ProductId = product1.ProductId,
                Quantity = 1,
                Price = product1.Price,
                Sale = sale1, // Установка связи с продажей
                Product = product1 // Установка связи с продуктом
            });

            db.SoldItems.Add(new SoldItem
            {
                SoldItemId = Guid.NewGuid(),
                SaleId = sale1.SaleId,
                ProductId = product2.ProductId,
                Quantity = 2,
                Price = product2.Price,
                Sale = sale1, // Установка связи с продажей
                Product = product2 // Установка связи с продуктом
            });

            db.SaveChanges();

            Console.WriteLine("Тестовые данные добавлены успешно!");
        }
    }
}
