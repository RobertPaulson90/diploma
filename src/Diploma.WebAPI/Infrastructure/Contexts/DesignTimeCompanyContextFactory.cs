using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Diploma.WebAPI.Infrastructure.Contexts
{
    public class DesignTimeCompanyContextFactory : IDesignTimeDbContextFactory<CompanyContext>
    {
        public CompanyContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CompanyContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlite(connectionString);

            return new CompanyContext(builder.Options);
        }
    }
}
