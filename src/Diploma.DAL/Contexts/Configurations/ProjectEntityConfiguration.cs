using Diploma.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("Projects");

            builder.Property(x => x.Foreit)
                .IsRequired();

            builder.Property(x => x.Foreit)
                .IsRequired();

            builder.Property(x => x.NumberOfMilestones)
                .IsRequired();

            builder.Property(x => x.PricePerHour)
                .IsRequired();

            builder.Property(x => x.ProjectType)
                .IsRequired();

            builder.Property(x => x.DeadlineDate)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired();
        }
    }
}
