using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PracticeArea
    {
        public int? PracticeAreaID { get; set; }
        public string PracticeAreaName { get; set; }
        public string PracticeAreaAcronym { get; set; }
        public string PracticeAreaAddress1 { get; set; }
        public string PracticeAreaAddress2 { get; set; }
        public string PracticeAreaCity { get; set; }
        public int? PracticeAreaStateID { get; set; }
        public string PracticeAreaZip { get; set; }
        public int? PracticeAreaCountryID { get; set; }
        public string PracticeAreaPhone { get; set; }
        public string PracticeAreaFax { get; set; }
        public string PracticeAreaEmail { get; set; }
        public string PracticeAreaUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool DeleteRecord { get; set; }
        public bool Available { get; set; }
        public int? UserID { get; set; }
        public string old_practiceareaid { get; set; }
        public decimal? practiceareaSalesGoal { get; set; }
        public decimal? MarginPercentage { get; set; }
        public decimal? AssessmentPercentage { get; set; }
        public string ROW_VERSION { get; set; }
    }
}