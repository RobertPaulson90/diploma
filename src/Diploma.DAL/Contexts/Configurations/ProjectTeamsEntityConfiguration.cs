using Diploma.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ProjectTeamsEntityConfiguration : IEntityTypeConfiguration<ProjectTeamEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectTeamEntity> builder)
        {
            builder.ToTable("ProjectTeams");

            builder.HasKey(
                x => new
                {
                    x.ProjectId,
                    x.TeamId
                });

            builder.HasOne(x => x.Project)
                .WithMany(x => x.InvolvedTeams)
                .HasForeignKey(x => x.ProjectId);

            builder.HasOne(x => x.Team)
                .WithMany(x => x.WorkingProjects)
                .HasForeignKey(x => x.TeamId);
        }
    }
}
