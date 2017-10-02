using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    public class TeamEntityConfiguration : EntityTypeConfiguration<TeamEntity>
    {
        public TeamEntityConfiguration()
        {
            ToTable("Teams");

            HasMany(x => x.InvolvedMembers).WithRequired(x => x.Team).HasForeignKey(x => x.TeamId);
        }
    }
}
