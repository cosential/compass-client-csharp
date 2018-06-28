using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelRegistration
    {
        public int? RegLookupID { get; set; }
        public string RegDisc { get; set; }
        public int? RegSt { get; set; }
        public string State { get; set; }
        public DateTime? RegDate { get; set; }
        public string RegLicNum { get; set; }
        public DateTime? FirstReg { get; set; }
        public DateTime? RegExp { get; set; }
        public bool StateIndicator { get; set; }
        public string Name { get; set; }
        public string old_reglookupid { get; set; }
        public int? CFirmID { get; set; }
        public string RegOrg { get; set; }
        public int? UserId { get; set; }
        public string legacyID { get; set; }
        public int? MembershipID { get; set; }
    }
}