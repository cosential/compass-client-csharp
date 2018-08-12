namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectComponentType
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        bool IsDeleted { get; set; }
        bool IsAvailable { get; set; }
        string Description { get; set; }
    }
}