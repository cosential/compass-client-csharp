using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Project
    {

        public Project() {

        }

        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string OtherProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjAddress { get; set; }
        public string ProjCity { get; set; }
        public string ProjState { get; set; }
        public string ProjZip { get; set; }
        public string ProjCounty { get; set; }
        public string ProjCountry { get; set; }
        public string MetroArea { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string FederalWork { get; set; }
        public bool Preferred { get; set; }
        public bool Prominent { get; set; }
        public string DescriptionBrief { get; set; }
        public string ProposalNumber { get; set; }
        public string SitePhoneNumber { get; set; }
        public float? PercentComplete { get; set; }
        public string ClientComments { get; set; }
        public string OwnerComments { get; set; }
        public bool CostConfidential { get; set; }
        public string Phase { get; set; }
        public DateTime? AwardedDate { get; set; }
        public DateTime? ConstructionStartDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public DateTime? EstCompletionDate { get; set; }
        public DateTime? EstConstructionStartDate { get; set; }
        public string ScheduleComments { get; set; }
        public int? BuildingSize { get; set; }
        public string SizeType { get; set; }
        public int? NumFloors { get; set; }
        public int? NumRooms { get; set; }
        public int? NumBeds { get; set; }
        public int? NumUnits { get; set; }
        public string SiteSize { get; set; }
        public string NewSize { get; set; }
        public int? RenovationSize { get; set; }
        public int? AdditionSize { get; set; }
        public int? ParkingSpaces { get; set; }
        public int? ParkingSize { get; set; }
        public string ProjAddress2 { get; set; }
        public string LogoFile { get; set; }
        public DateTime? ConstructionCompletionDate { get; set; }
        public string Subsidiary { get; set; }
        public int? DeleteRecord { get; set; }
        public string SizeComments { get; set; }
        public int? ProfileCodeId { get; set; }
        public int? Bonded { get; set; }
        public string Notes { get; set; }
        public string OwnerType { get; set; }
        public string FileStatus { get; set; }
        public string ProjectsShortText1 { get; set; }
        public string ProjectsShortText2 { get; set; }
        public string ProjectsShortText3 { get; set; }
        public string ProjectsShortText4 { get; set; }
        public string ProjectsShortText5 { get; set; }
        public int? ProjectsNumber1 { get; set; }
        public int? ProjectsNumber2 { get; set; }
        public int? ProjectsNumber3 { get; set; }
        public int? ProjectsNumber4 { get; set; }
        public int? ProjectsNumber5 { get; set; }
        public DateTime? ProjectsDate1 { get; set; }
        public DateTime? ProjectsDate2 { get; set; }
        public DateTime? ProjectsDate3 { get; set; }
        public DateTime? ProjectsDate4 { get; set; }
        public DateTime? ProjectsDate5 { get; set; }
        public string ProjectsLongText1 { get; set; }
        public string ProjectsLongText2 { get; set; }
        public string ProjectsLongText3 { get; set; }
        public string ProjectsLongText4 { get; set; }
        public string ProjectsLongText5 { get; set; }
        public int? ProjectsValueListID1 { get; set; }
        public int? ProjectsValueListID2 { get; set; }
        public int? ProjectsValueListID3 { get; set; }
        public int? ProjectsValueListID4 { get; set; }
        public int? ProjectsValueListID5 { get; set; }
        public float? ProjectsMoney1 { get; set; }
        public float? ProjectsMoney2 { get; set; }
        public float? ProjectsMoney3 { get; set; }
        public float? ProjectsMoney4 { get; set; }
        public float? ProjectsMoney5 { get; set; }
        public string Accounting_ID { get; set; }
        public bool UsesExternalData { get; set; }
        public float? FeeBasisFee { get; set; }
        public float? FeeBasisRate { get; set; }
        public float? FeeBasisUpset { get; set; }
        public float? InternalLabor { get; set; }
        public float? FeesEarned { get; set; }
        public string SiteFax { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingCountry { get; set; }
        public string BillingCounty { get; set; }
        public string BillingPhone { get; set; }
        public string BillingFax { get; set; }
        public float? ProjectsPercent1 { get; set; }
        public float? ProjectsPercent2 { get; set; }
        public float? ProjectsPercent3 { get; set; }
        public float? ProjectsPercent4 { get; set; }
        public float? ProjectsPercent5 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedByUser { get; set; }
        public string ScheduleScheduleComments { get; set; }
        public DateTime? ScheduleConstCompletionDate { get; set; }
        public string ProjStatus { get; set; }
        public int? ProjectStatusID { get; set; }
        public bool PublishToWebsite { get; set; }
        public string ROW_VERSION { get; set; }
        public int? OpportunityId { get; set; }
        public string ProjectDisplayName { get; set; }
        public int? MasterID { get; set; }
        public bool isMarketingProject { get; set; }
        public bool isSubMarketingProject { get; set; }
        public string Role { get; set; }
        public bool IsPublishableProject { get; set; }
    }
}
