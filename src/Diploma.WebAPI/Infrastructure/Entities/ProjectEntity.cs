using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.WebAPI.Infrastructure.Entities
{
    public class ProjectEntity
    {
        public virtual CustomerEntity Customer { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public DateTime DeadlineDate { get; set; }

        [Required]
        public decimal Foreit { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ICollection<ProjectTeamEntity> InvolvedTeams { get; set; }

        public virtual ManagerEntity Manager { get; set; }

        public int ManagerId { get; set; }

        [Required]
        public int NumberOfMilestones { get; set; }

        [Required]
        public decimal PricePerHour { get; set; }

        [Required]
        public string ProjectType { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
