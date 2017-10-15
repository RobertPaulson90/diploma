using Diploma.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class TeamEntityConfiguration : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.ToTable("Teams");

            builder.HasMany(x => x.InvolvedMembers)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId);
        }
    }
}
