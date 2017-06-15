namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffTeam
    {

        public StaffTeam()
        {

        }

        public int? StaffTeamId { get; set; }
        public int? StaffTeamRoleId { get; set; }
        public int? PersonnelId { get; set; }
        public int? OpportunityId { get; set; }
        public int? PercentInvolved { get; set; }
        public int? SalesCredit { get; set; }
        public string Description { get; set; }
        public int? SF330_TeamInd { get; set; }
        public int? OppLeadAssignedToId { get; set; }

    }
}