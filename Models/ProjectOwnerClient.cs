namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectOwnerClient
    {
        public int? Id { get; set; }
        public Company Company { get; set; }
        public string Comments { get; set; }
        public string LegacyRole { get; set; }
    }
}