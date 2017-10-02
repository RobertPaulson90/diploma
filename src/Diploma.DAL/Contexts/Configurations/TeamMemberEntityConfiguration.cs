using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    public class TeamMemberEntityConfiguration : EntityTypeConfiguration<TeamMemberEntity>
    {
        public TeamMemberEntityConfiguration()
        {
            ToTable("TeamMembers");
        }
    }
}
