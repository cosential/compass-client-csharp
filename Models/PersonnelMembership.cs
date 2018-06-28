using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelMembership
    {
        public int? MembershipID { get; set; }
        public int? MembershipTypeID { get; set; }
        public string MembershipTitle { get; set; }
        public int? cFirmID { get; set; }
        public DateTime? MembershipDate { get; set; }
        public string MembershipTerm { get; set; }
        public DateTime? RenewalDate { get; set; }
        public bool RenewalNotice { get; set; }
        public string Description { get; set; }
        public string legacyID { get; set; }
        public string MembershipCompanyTextName { get; set; }
        public int? UserID { get; set; }
        public string MembershipTypeName { get; set; }
        public int? DegreeId { get; set; }
        public int? SF330_DegreeInd { get; set; }
        public int? UserId { get; set; }
    }
}