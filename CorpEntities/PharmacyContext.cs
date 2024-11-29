using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CorpEntities
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Проверяем, был ли уже настроен контекст
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database= appdb;Persist Security Info=True;User ID=postgres;Password=postgres",
                    options => options
                        .MigrationsAssembly("CorpMigration")  //Указываем сборку миграций
                        .MigrationsHistoryTable("PharmacyMigrations", "public") //Таблица миграций в схеме public
                        .CommandTimeout(1000));  
            }

            //Остальные настройки
            optionsBuilder.UseLazyLoadingProxies()
                          .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Устанавливаем схему по умолчанию для всех таблиц
            modelBuilder.HasDefaultSchema("pharmacy");

            //Уникальные индексы
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SoldItem> SoldItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
