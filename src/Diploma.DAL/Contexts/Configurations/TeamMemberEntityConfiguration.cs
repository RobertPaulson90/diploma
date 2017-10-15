using Diploma.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class TeamMemberEntityConfiguration : IEntityTypeConfiguration<TeamMemberEntity>
    {
        public void Configure(EntityTypeBuilder<TeamMemberEntity> builder)
        {
            builder.ToTable("TeamMembers");

            builder.Property(x => x.TeamRole)
                .IsRequired();
        }
    }
}
