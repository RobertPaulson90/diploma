using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ManagerEntityConfiguration : EntityTypeConfiguration<ManagerEntity>
    {
        public ManagerEntityConfiguration()
        {
            HasMany(x => x.ManagedProjects)
                .WithRequired(x => x.Manager)
                .HasForeignKey(x => x.ManagerId);
        }
    }
}
