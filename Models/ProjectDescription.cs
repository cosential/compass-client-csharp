namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectDescription
    {
        public int? DescriptionId { get; set; }
        public int? DescriptionTypeId { get; set; }
        public string DescriptionType { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool IsTimeSensitive { get; set; }
        public bool IsWebsite { get; set; }
        public bool IsResume { get; set; }
        public bool IsSf330 { get; set; }
    }
}