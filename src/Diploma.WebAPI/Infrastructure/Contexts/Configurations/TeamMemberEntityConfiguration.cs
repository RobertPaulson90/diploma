using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.WebAPI.Infrastructure.Contexts.Configurations
{
    internal sealed class TeamMemberEntityConfiguration : IEntityTypeConfiguration<TeamMemberEntity>
    {
        public void Configure(EntityTypeBuilder<TeamMemberEntity> builder)
        {
            builder.ToTable("TeamMembers");
        }
    }
}
