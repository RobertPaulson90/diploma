using System.Data.Entity;
using Diploma.DAL.Entities;
using SQLite.CodeFirst;

namespace Diploma.DAL.Contexts
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()
            : base("CompanyDb")
        {
        }

        public DbSet<AdminEntity> Admins { get; set; }
        
        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<EmployeeEntity> Employees { get; set; }

        public DbSet<ManagerEntity> Managers { get; set; }

        public DbSet<ProgrammerEntity> Programmers { get; set; }

        public DbSet<ProjectEntity> Projects { get; set; }

        public DbSet<TeamMemberEntity> TeamMembers { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(CompanyContext).Assembly);

            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CompanyContext>(modelBuilder, true);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
