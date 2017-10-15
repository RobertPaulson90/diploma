namespace Diploma.DAL.Entities
{
    public abstract class EmployeeEntity : UserEntity
    {
        public decimal Salary { get; set; }
    }
}
