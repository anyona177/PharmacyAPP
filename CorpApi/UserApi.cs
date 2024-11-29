using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorpEntities;

namespace CorpApi
{
    public static class UserApi
    {
        public static void MapUserApi(this WebApplication app)
        {
            // Регистрация нового пользователя
            app.MapPost("/register", async (RegisterUserDto request, PharmacyContext db) =>
            {
                // Проверяем, существует ли сотрудник
                var existingEmployee = await db.Employees
                    .FirstOrDefaultAsync(e => e.Name == request.Name && e.Surname == request.Surname && e.Phone == request.Phone);

                Employee employee;

                if (existingEmployee == null)
                {
                    // Создаем нового сотрудника
                    employee = new Employee
                    {
                        Name = request.Name,
                        Surname = request.Surname,
                        Patronymic = request.Patronymic,
                        Post = request.Post,
                        Phone = request.Phone
                    };

                    db.Employees.Add(employee);
                    await db.SaveChangesAsync();
                }
                else
                {
                    employee = existingEmployee;
                }

                // Проверяем, существует ли пользователь с таким логином
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    return Results.BadRequest("Пользователь с таким логином уже существует.");
                }

                // Создаем пользователя
                var hashedPassword = PasswordHasher.HashPassword(request.Password);
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                    EmployeeId = employee.EmployeeId,
                    Employee = employee // Обязательное свойство инициализировано
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                return Results.Created($"/users/{newUser.UserId}", newUser);
            });


            // Вход пользователя
            app.MapPost("/login", async (LoginUserDto request, PharmacyContext db, JwtTokenService tokenService) =>
            {
                var user = await db.Users
                    .Include(u => u.Employee)
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                    return Results.BadRequest("Неверное имя пользователя или пароль.");

                var token = tokenService.GenerateToken(user);
                return Results.Ok(new { Token = token });
            });
        }
    }

    public static class PasswordHasher
    {
        // Метод для хеширования пароля
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            using var sha512 = SHA512.Create();
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hash).Replace("-", "").ToLower(); // Конвертация в строку в нижнем регистре
        }

        // Метод для проверки пароля
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }

    // DTO для регистрации
    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Post { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    // DTO для входа
    public class LoginUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
