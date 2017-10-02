using System.Data.Entity.ModelConfiguration;
using Diploma.DAL.Entities;

namespace Diploma.DAL.Contexts.Configurations
{
    public class EmployeeEntityConfiguration : EntityTypeConfiguration<EmployeeEntity>
    {
        public EmployeeEntityConfiguration()
        {
            ToTable("Employees");

            Map<ProgrammerEntity>(m => m.ToTable("Programmers"));

            Map<ManagerEntity>(m => m.ToTable("Managers"));
        }
    }
}
