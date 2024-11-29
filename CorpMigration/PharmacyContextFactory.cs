using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CorpEntities;

namespace CorpMigration
{
    internal class PharmacyContextFactory : IDesignTimeDbContextFactory<PharmacyContext>
    {
        static PharmacyContextFactory() =>
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        public PharmacyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyContext>();

            const string connectionString =
                "Host=localhost;Port=5433;Database= appdb;Persist Security Info=True;User ID=postgres;Password=postgres";

            optionsBuilder
                .UseNpgsql(
                    connectionString,
                    options => options
                        .CommandTimeout(1000)
                        .MigrationsAssembly("CorpMigration")
                        .MigrationsHistoryTable("PharmacyMigrations", "public"));

            return new PharmacyContext(optionsBuilder.Options);
        }
    }
}
