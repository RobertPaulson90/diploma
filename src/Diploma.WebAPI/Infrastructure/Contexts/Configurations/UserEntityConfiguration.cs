using Diploma.WebAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diploma.WebAPI.Infrastructure.Contexts.Configurations
{
    internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.HasDiscriminator<string>("Role")
                .HasValue<CustomerEntity>("Customer")
                .HasValue<ProgrammerEntity>("Programmer")
                .HasValue<ManagerEntity>("Manager")
                .HasValue<AdminEntity>("Admin");
        }
    }
}
