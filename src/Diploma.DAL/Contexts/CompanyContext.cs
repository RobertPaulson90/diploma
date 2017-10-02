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

        public DbSet<CredentialsEntity> Credentials { get; set; }

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
            modelBuilder.Entity<EmployeeEntity>().ToTable("Employees").Map<ProgrammerEntity>(m => m.ToTable("Programmers"))
                .Map<ManagerEntity>(m => m.ToTable("Managers"));

            modelBuilder.Entity<ProgrammerEntity>().HasMany(x => x.TeamsMemberships).WithRequired(x => x.Programmer)
                .HasForeignKey(x => x.ProgrammerId);

            modelBuilder.Entity<ManagerEntity>().HasMany(x => x.ManagedProjects).WithRequired(x => x.Manager).HasForeignKey(x => x.ManagerId);

            modelBuilder.Entity<CustomerEntity>().ToTable("Customers").HasMany(x => x.Projects).WithRequired(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<ProjectEntity>().ToTable("Projects").HasMany(x => x.InvolvedTeams).WithMany(x => x.WorkingProjects).Map(
                x =>
                {
                    x.ToTable("ProjectTeams");
                    x.MapLeftKey("ProjectId");
                    x.MapRightKey("TeamId");
                });

            modelBuilder.Entity<TeamEntity>().ToTable("Teams").HasMany(x => x.InvolvedMembers).WithRequired(x => x.Team).HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<TeamMemberEntity>().ToTable("TeamMembers");

            modelBuilder.Entity<UserEntity>().ToTable("Users").HasRequired(x => x.Credentials).WithRequiredPrincipal(x => x.User);

            modelBuilder.Entity<AdminEntity>().ToTable("Admins");

            modelBuilder.Entity<CredentialsEntity>().ToTable("Credentials");

            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<CompanyContext>(modelBuilder, true);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
