using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.WebAPI.Infrastructure.Contexts.Configurations
{
    internal sealed class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("Projects");
        }
    }
}
