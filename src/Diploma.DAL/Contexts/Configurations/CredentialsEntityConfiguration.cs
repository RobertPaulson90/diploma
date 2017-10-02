using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    public class CredentialsEntityConfiguration : EntityTypeConfiguration<CredentialsEntity>
    {
        public CredentialsEntityConfiguration()
        {
            ToTable("Credentials");
        }
    }
}