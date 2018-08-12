using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffRole
    {
        public int? StaffRoleId { get; set; }
        public string StaffRoleName { get; set; }
        public string StaffRoleType { get; set; }
        public string Description { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public int? ExternalId { get; set; }
    }
}