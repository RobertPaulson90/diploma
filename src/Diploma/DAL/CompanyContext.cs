using System.Data.Entity;
using Diploma.Entities;
using SQLite.CodeFirst;

namespace Diploma.DAL
{
    public class CompanyContext : DbContext
    {
        public CompanyContext() : base("company_db")
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Programmer> Programmers { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees")
                .Map<Programmer>(m => m.ToTable("Programmers"))
                .Map<Manager>(m => m.ToTable("Managers"));

            modelBuilder.Entity<Programmer>()
                .HasMany(x => x.TeamsMemberships)
                .WithRequired(x => x.Programmer)
                .HasForeignKey(x => x.ProgrammerId);

            modelBuilder.Entity<Manager>()
                .HasMany(x => x.ManagedProjects)
                .WithRequired(x => x.Manager)
                .HasForeignKey(x => x.ManagerId);

            modelBuilder.Entity<Customer>()
                .ToTable("Customers")
                .HasMany(x => x.Projects)
                .WithRequired(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<Project>()
                .HasMany(x => x.InvolvedTeams)
                .WithMany(x => x.WorkingProjects)
                .Map(x =>
                {
                    x.ToTable("ProjectTeams");
                    x.MapLeftKey("ProjectId");
                    x.MapRightKey("TeamId");
                });

            modelBuilder.Entity<Team>()
                .HasMany(x => x.InvolvedMembers)
                .WithRequired(x => x.Team)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<TeamMember>();

            modelBuilder.Entity<User>();

            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CompanyContext>(modelBuilder, true);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}