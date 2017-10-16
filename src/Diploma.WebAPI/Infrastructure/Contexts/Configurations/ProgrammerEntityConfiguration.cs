using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.WebAPI.Infrastructure.Contexts.Configurations
{
    internal sealed class ProgrammerEntityConfiguration : IEntityTypeConfiguration<ProgrammerEntity>
    {
        public void Configure(EntityTypeBuilder<ProgrammerEntity> builder)
        {
            builder.HasMany(x => x.TeamsMemberships)
                .WithOne(x => x.Programmer)
                .HasForeignKey(x => x.ProgrammerId);
        }
    }
}
