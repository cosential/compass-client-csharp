using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelPresentation
    {
        public int? PresentationId { get; set; }
        public string PresentationTitle { get; set; }
        public DateTime? PresentationDate { get; set; }
        public string Description { get; set; }
    }
}