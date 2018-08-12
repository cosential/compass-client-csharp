namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectStatus
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}