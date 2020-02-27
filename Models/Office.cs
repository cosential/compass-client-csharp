using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Office
    {
        public int? OfficeID { get; set; }
        public string OfficeName { get; set; }
        public bool DeleteIndicator { get; set; }
        public string OfficeAcronym { get; set; }
        public int? HeadquarterIndicator { get; set; }
        public string OfficeAddress1 { get; set; }
        public string OfficeAddress2 { get; set; }
        public string OfficeCity { get; set; }
        public int? OfficeStateID { get; set; }
        public string OfficeZip { get; set; }
        public string OfficeCounty { get; set; }
        public int? OfficeCountryID { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeEmail { get; set; }
        public string OfficeURL { get; set; }
        public string FederalID { get; set; }
        public string DunsID { get; set; }
        public string AcassID { get; set; }
        public string BusinessLicense { get; set; }
        public int? BusinessStatusID { get; set; }
        public int? TotalStaffNum { get; set; }
        public string InsuranceCoverage { get; set; }
        public string InsuranceCarrier { get; set; }
        public int? OwnershipID { get; set; }
        public string PreviousFirmNames { get; set; }
        public string YearFounded { get; set; }
        public int? PreviousFirm_YearFounded { get; set; }
        public string PreviousFirm_DunsID { get; set; }
        public string ServicesProvided { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? UserID { get; set; }
        public string old_officeid { get; set; }
        public decimal? officeSalesGoal { get; set; }
        public decimal? MarginPercentage { get; set; }
        public decimal? AssessmentPercentage { get; set; }
        public decimal? GoPercent { get; set; }
        public decimal? GetPercent { get; set; }
        public int? SelfPerformHours { get; set; }
        public decimal? FeeCIPercent { get; set; }
        public int? LaborDifferential { get; set; }
        public bool? Available { get; set; }
        public string ROW_VERSION { get; set; }
    }
}