namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffTeam
    {

        public StaffTeam() {

        }

        public int? OppStaffTeamID { get; set; }
        public int? PercentInvolved { get; set; }
        public int? SalesCredit { get; set; }
        public string Description { get; set; }
        public int? SF330_TeamInd { get; set; }
        public long PersonnelId { get; set; }
        public long StaffRoleId { get; set; }

    }
}