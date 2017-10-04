using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class ProgrammerEntityConfiguration : EntityTypeConfiguration<ProgrammerEntity>
    {
        public ProgrammerEntityConfiguration()
        {
            HasMany(x => x.TeamsMemberships).WithRequired(x => x.Programmer).HasForeignKey(x => x.ProgrammerId);
        }
    }
}
