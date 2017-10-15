using Diploma.DAL.Contexts.Configurations;
using Diploma.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diploma.DAL.Contexts
{
    public sealed class CompanyContext : IdentityDbContext<ApplicationUser>
    {
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> UserEntities { get; set; }

        public DbSet<CustomerEntity> CustomerEntities { get; set; }

        public DbSet<EmployeeEntity> EmployeeEntities { get; set; }

        public DbSet<ManagerEntity> ManagerEntities { get; set; }

        public DbSet<ProgrammerEntity> ProgrammerEntities { get; set; }

        public DbSet<ProjectEntity> ProjectEntities { get; set; }

        public DbSet<TeamMemberEntity> TeamMemberEntities { get; set; }

        public DbSet<TeamEntity> TeamEntities { get; set; }
        
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
