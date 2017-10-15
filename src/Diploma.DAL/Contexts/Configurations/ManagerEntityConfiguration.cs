using Diploma.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ManagerEntityConfiguration : IEntityTypeConfiguration<ManagerEntity>
    {
        public void Configure(EntityTypeBuilder<ManagerEntity> builder)
        {
            builder.HasMany(x => x.ManagedProjects)
                .WithOne(x => x.Manager)
                .HasForeignKey(x => x.ManagerId);
        }
    }
}
