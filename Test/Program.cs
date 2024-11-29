using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CorpEntities;

namespace Test
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
           
            // Получение существующего сотрудника
            var employee = db.Employees.FirstOrDefault();
            if (employee == null)
            {
                Console.WriteLine("Нет сотрудников в базе данных.");
                return;
            }

            // Получение существующих продуктов
            var products = db.Products.ToList();
            if (products.Count < 2) // Убедитесь, что есть хотя бы два продукта
            {
                Console.WriteLine("Недостаточно продуктов в базе данных.");
                return;
            }

            // Указание времени продажи в нужном формате
            var saleDateString = "2024-10-10 11:09:44";
            DateTime saleDate;
            if (!DateTime.TryParseExact(saleDateString, "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out saleDate))
            {
                Console.WriteLine("Неверный формат даты.");
                return;
            }

            // Создание продажи с явным указанием SaleId
            var sale1 = new Sale
            {
                SaleId = 6, // Укажите здесь нужный SaleId
                EmployeeId = employee.EmployeeId,
                Employee = employee,
                SaleDate = saleDate // Используем заданное время
            };

            // Добавление проданных товаров
            var soldItem1 = new SoldItem
            {
                Sale = sale1,
                Product = products[1], // Используем существующий продукт
                Quantity = 2,
                Price = products[1].Price
            };

            var soldItem2 = new SoldItem
            {
                Sale = sale1,
                Product = products[5], // Используем существующий продукт
                Quantity = 1,
                Price = products[5].Price
            };
            var soldItem3 = new SoldItem
            {
                Sale = sale1,
                Product = products[4], // Используем существующий продукт
                Quantity = 1,
                Price = products[4].Price
            };

            // Добавление проданных товаров к продаже
            sale1.SoldItems.Add(soldItem1);
            sale1.SoldItems.Add(soldItem2);
            sale1.SoldItems.Add(soldItem3);
            // Вычисление общей суммы продажи
            sale1.TotalAmount = sale1.SoldItems.Sum(item => item.Quantity * item.Price);

            // Теперь добавляем продажу
            db.Sales.Add(sale1);
            db.SaveChanges();

            Console.WriteLine("Тестовые данные добавлены успешно!");
        }
    }
}
    

