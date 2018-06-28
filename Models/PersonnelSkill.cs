using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelSkill
    {
        public int? SkillID { get; set; }
        public int? UserID { get; set; }
        public int? SkillTypeID { get; set; }
        public int? CFirmID { get; set; }
        public string SkillName { get; set; }
        public string SkillDesc { get; set; }
        public string SkillLevel { get; set; }
        public bool Certified { get; set; }
        public string CertOrganization { get; set; }
        public DateTime? DateCertified { get; set; }
        public DateTime? RenewDate { get; set; }
        public string legacyID { get; set; }
        public string CertCompanyTextName { get; set; }
    }
}