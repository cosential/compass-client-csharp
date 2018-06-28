using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelAward
    {
        public int? AwardId { get; set; }
        public string Name { get; set; }
        public bool IsPrefered { get; set; }
        public DateTime? YearAwarded { get; set; }
    }
}