using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelTraining
    {
        public int? TrainingId { get; set; }
        public int PersonnelId { get; set; }
        public string Goal { get; set; }
        public string Course { get; set; }
        public string Sponsor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Hours { get; set; }
        public string Credits { get; set; }
        public decimal? Cost { get; set; }
        public bool CertRcvd { get; set; }
        public DateTime? RenewDate { get; set; }
        public int? FsclYr { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public int? TrainingStatusID { get; set; }
    }
}