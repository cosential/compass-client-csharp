using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectStaffTeam
    {
        public int? Id { get; set; }
        public Personnel Personnel { get; set; }
        public StaffRole StaffRole { get; set; }
        public int? PercentInvolved { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public string Description { get; set; }
        public bool PccAccess { get; set; }
        public float? StaffTeamHours { get; set; }
        public DateTime? InvolvementStartDate { get; set; }
        public DateTime? InvolvementEndDate { get; set; }
    }
}