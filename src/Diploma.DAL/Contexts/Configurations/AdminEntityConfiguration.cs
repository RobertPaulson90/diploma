using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class AdminEntityConfiguration : EntityTypeConfiguration<AdminEntity>
    {
        public AdminEntityConfiguration()
        {
            ToTable("Admins");
        }
    }
}
