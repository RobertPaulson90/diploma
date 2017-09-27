using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diploma.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int TimeForCompletion { get; set; }

        [Required]
        public decimal PricePerHour { get; set; }

        [Required]
        public decimal Foreit { get; set; }

        public string ProjectType { get; set; }

        [Required]
        public int NumberOfMilestones { get; set; }

        public virtual Customer Customer { get; set; }
        
        public virtual Manager Manager { get; set; }
        
        public virtual ICollection<Team> InvolvedTeams { get; set; }

        public int ManagerId { get; set; }

        public int CustomerId { get; set; }
    }
}