namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectConsultant
    {
        public int? Id { get; set; }
        public Company Company { get; set; }
        public ProjectConsultantRole ConsultantRole { get; set; }
        public string Comments { get; set; }
    }
}