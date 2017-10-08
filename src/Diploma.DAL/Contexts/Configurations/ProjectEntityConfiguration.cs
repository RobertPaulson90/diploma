using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ProjectEntityConfiguration : EntityTypeConfiguration<ProjectEntity>
    {
        public ProjectEntityConfiguration()
        {
            ToTable("Projects");

            HasMany(x => x.InvolvedTeams)
                .WithMany(x => x.WorkingProjects)
                .Map(
                    x =>
                    {
                        x.ToTable("ProjectTeams");
                        x.MapLeftKey("ProjectId");
                        x.MapRightKey("TeamId");
                    });
        }
    }
}
