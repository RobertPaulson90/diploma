namespace Diploma.DAL.Entities
{
    public class ProjectTeamEntity
    {
        public virtual ProjectEntity Project { get; set; }

        public int ProjectId { get; set; }

        public virtual TeamEntity Team { get; set; }

        public int TeamId { get; set; }
    }
}
