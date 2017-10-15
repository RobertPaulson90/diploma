using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.DAL.Entities
{
    public class ProjectEntity
    {
        public virtual CustomerEntity Customer { get; set; }

        public int CustomerId { get; set; }

        public DateTime DeadlineDate { get; set; }

        public decimal Foreit { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ICollection<ProjectTeamEntity> InvolvedTeams { get; set; }

        public virtual ManagerEntity Manager { get; set; }

        public int ManagerId { get; set; }

        public int NumberOfMilestones { get; set; }

        public decimal PricePerHour { get; set; }

        public string ProjectType { get; set; }

        public string Title { get; set; }
    }
}
