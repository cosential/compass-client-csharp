using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffTeam
    {
        public long OppStaffTeamID { get; set; }
        public int? OpportunityId { get; set; }
        public int? PercentInvolved { get; set; }
        public int? SalesCredit { get; set; }
        public string Description { get; set; }
        public int? SF330_TeamInd { get; set; }

        public StaffTeamRole StaffRole { get; set; }
        public PersonnelSummary Personnel { get; set; }
    }
}