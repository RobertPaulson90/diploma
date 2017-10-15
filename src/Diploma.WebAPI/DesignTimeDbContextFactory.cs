using System.IO;
using Diploma.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Diploma.WebAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CompanyContext>
    {
        public CompanyContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CompanyContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlite(connectionString, SqliteOptionsAction);

            return new CompanyContext(builder.Options);
        }

        private void SqliteOptionsAction(SqliteDbContextOptionsBuilder sqliteDbContextOptionsBuilder)
        {
        }
    }
}