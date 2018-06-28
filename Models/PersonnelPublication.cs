using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelPublication
    {
        public int? PublicationId { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Pages { get; set; }
        public string Publication { get; set; }
        public string Title { get; set; }
        public DateTime? YearPublished { get; set; }
    }
}