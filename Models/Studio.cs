using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Studio
    {
        public int? StudioID { get; set; }
        public string StudioName { get; set; }
        public string StudioAcronym { get; set; }
        public string StudioAddress1 { get; set; }
        public string StudioAddress2 { get; set; }
        public string StudioCity { get; set; }
        public int? StudioStateID { get; set; }
        public string StudioZip { get; set; }
        public int? StudioCountryID { get; set; }
        public string StudioPhone { get; set; }
        public string StudioFax { get; set; }
        public string StudioEmail { get; set; }
        public string StudioUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool DeleteRecord { get; set; }
        public bool Available { get; set; }
        public int? UserID { get; set; }
        public string old_studioid { get; set; }
        public decimal? StudioSalesGoal { get; set; }
        public decimal? MarginPercentage { get; set; }
        public decimal? AssessmentPercentage { get; set; }
        public string AssociatedServices { get; set; }
    }
}
