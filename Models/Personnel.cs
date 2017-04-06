using System;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Personnel
    {
        public Personnel()
        {
            //defaults
            Status = "Active";
        }

        public int? PersonnelId { get; set; } //Cosential's primary key
        public string ExternalId { get; set; } //Customer assigned employee number
        public string FirstName { get; set; }
        public string MI { get; set; } //Middle name, not middle initial
        public string LastName { get; set; }
        public string Status { get; set; }
        public int? SupervisorId { get; set; } //Cosential's PersonnelId for the supervisor
        public string Title { get; set; }
        public DateTime? StartDate { get; set; } //Hire date
        public DateTime? EndDate { get; set; } //Termination date
        public string OfficeEmail { get; set; }
        public string OfficePhone { get; set; }
        public string OfficePhoneExtension { get; set; }
        public string NickName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool DeleteInd { get; set; }
        public string ProfDesc1 { get; set; }
        public string ProfDesc2 { get; set; }
        public string FormalName { get; set; }
        public string HomeAddress1 { get; set; }
        public string HomeAddress2 { get; set; }
        public string HomeCity { get; set; }
        public string HomeState { get; set; }
        public string HomeCountry { get; set; }
        public string HomeZip { get; set; }
        public string HomeFax { get; set; }
        public string OfficeCellPhone { get; set; }
        public string PersonalCellPhone { get; set; }
        public string HomeEmail { get; set; }
        public string HomePhone { get; set; }
        public bool TechStaff { get; set; }
        public string OtherCat { get; set; }
        public string OfficeFax { get; set; }
        public string CellPhone { get; set; }
        public string Partner { get; set; }
        public string Nationality { get; set; }
        public string Acronym { get; set; }
        public string Prefix { get; set; }
        public string OfficePager { get; set; }
        public string Suffix { get; set; }
        public int? SF330_OfficeID { get; set; }
        public int? EEOCID { get; set; }
        public string Gender { get; set; }
        public int? Race { get; set; }
        public int? AffirmativeActionID { get; set; }
        public int? Ethnicity { get; set; }
        public int? MaritalStatus { get; set; }
        public int? MilitaryStatus { get; set; }
        public int? VisaStatus { get; set; }
        public DateTime? Visa_ExpDate { get; set; }
        public string Notes { get; set; }
        public string bio { get; set; }
        public int? HRStatusID { get; set; }
        public string GovernmentClearances { get; set; }
        public string SummaryNotes { get; set; }
        public int? Year { get; set; }
        public DateTime? PreviousStartDate { get; set; }
        public DateTime? PreviousSeparationDate { get; set; }
        public DateTime? LastPromotionDate { get; set; }
        public float? AdditionalTimeWithFirm { get; set; }
        public int? YearsWithOtherFirms { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? LastDeletedDate { get; set; }
        public int? Discipline_330CodeId { get; set; }
        public string EmergencyContact1Name { get; set; }
        public string EmergencyContact1Phone { get; set; }
        public string EmergencyContact1Relationship { get; set; }
        public string EmergencyContact1CellPhone { get; set; }
        public string EmergencyContact1Address1 { get; set; }
        public string EmergencyContact1Address2 { get; set; }
        public string EmergencyContact1City { get; set; }
        public string EmergencyContact1State { get; set; }
        public string EmergencyContact1Country { get; set; }
        public string EmergencyContact1Zip { get; set; }
        public string EmergencyContact2Name { get; set; }
        public string EmergencyContact2Phone { get; set; }
        public string EmergencyContact2Relationship { get; set; }
        public string EmergencyContact2CellPhone { get; set; }
        public string EmergencyContact2Address1 { get; set; }
        public string EmergencyContact2Address2 { get; set; }
        public string EmergencyContact2City { get; set; }
        public string EmergencyContact2State { get; set; }
        public string EmergencyContact2Country { get; set; }
        public string EmergencyContact2Zip { get; set; }
        public string Level { get; set; }
        public string DeskOfficeLocation { get; set; }
        public int? SF254Code { get; set; }
        public bool Confidential { get; set; }
        public string SF330DisciplineCode { get; set; }
        public string Supervisor { get; set; }
        public int? DisciplineCodeId { get; set; }

    }
}