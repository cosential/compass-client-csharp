using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Division
    {
        public int? DivisionID { get; set; }
        public string DivisionName { get; set; }
        //public bool DeleteIndicator { get; set; }
        public string DivisionAccronym { get; set; }
        public string DivisionAddress1 { get; set; }
        public string DivisionAddress2 { get; set; }
        public string DivisionCity { get; set; }
        public int? DivisionStateID { get; set; }
        public string DivisionZip { get; set; }
        public int? DivisionCountryID { get; set; }
        public string DivisionPhone { get; set; }
        public string DivisionFax { get; set; }
        public string DivisionEmail { get; set; }
        public string DivisionURL { get; set; }
        public string DunsID { get; set; }
        public int? OwnershipID { get; set; }
        public string BusinessLicense { get; set; }
        public int? BusinessStatusID { get; set; }
        public int? YearFounded { get; set; }
        public int? TotalStaffNum { get; set; }
        public string PreviousDivisionNames { get; set; }
        public string PreviousDivision_YearFounded { get; set; }
        public string PreviousDivision_DunsID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool DeleteRecord { get; set; }
        public bool Available { get; set; }
        public int? UserID { get; set; }
        public string old_divisionid { get; set; }
        public decimal? divisionSalesGoal { get; set; }
        public decimal? MarginPercentage { get; set; }
        public decimal? AssessmentPercentage { get; set; }
        public string ROW_VERSION { get; set; }
    }
}