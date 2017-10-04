using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    internal sealed class CustomerEntityConfiguration : EntityTypeConfiguration<CustomerEntity>
    {
        public CustomerEntityConfiguration()
        {
            ToTable("Customers");

            HasMany(x => x.Projects).WithRequired(x => x.Customer).HasForeignKey(x => x.CustomerId);
        }
    }
}
