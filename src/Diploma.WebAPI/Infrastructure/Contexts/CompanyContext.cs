using Diploma.WebAPI.Infrastructure.Contexts.Configurations;
using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diploma.WebAPI.Infrastructure.Contexts
{
    public sealed class CompanyContext : DbContext
    {
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<EmployeeEntity> Employees { get; set; }

        public DbSet<ManagerEntity> Managers { get; set; }

        public DbSet<ProgrammerEntity> Programmers { get; set; }

        public DbSet<ProjectEntity> Projects { get; set; }

        public DbSet<TeamMemberEntity> TeamMembers { get; set; }

        public DbSet<TeamEntity> Teams { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ManagerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProgrammerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTeamsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMemberEntityConfiguration());
        }
    }
}
