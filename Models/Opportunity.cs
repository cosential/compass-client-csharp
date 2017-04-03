using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Opportunity
    {

        public Opportunity()
        {

        }

        public int? OpportunityId { get; set; }
        public int? ClientId { get; set; }
        public int? SF255Form { get; set; }
        public int? SF330Form { get; set; }
        public string OpportunityName { get; set; }
        public DateTime? EstimatedSelectionDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public float? Cost { get; set; }
        public int? Size { get; set; }
        public string SizeUnit { get; set; }
        public float? Fee { get; set; }
        public int? Probability { get; set; }
        public string NextAction { get; set; }
        public bool RFPRecieved { get; set; }
        public string ProposalNumber { get; set; }
        public DateTime? ProposalDueDate { get; set; }
        public DateTime? ExpectedRFPDate { get; set; }
        public DateTime? QualsDueDate { get; set; }
        public bool ProposalSubmitted { get; set; }
        public DateTime? PresentationDate { get; set; }
        public string Comments { get; set; }
        public float? MarketingCostBudget { get; set; }
        public float? MarketingCostActual { get; set; }
        public string OpportunityNumber { get; set; }
        public int? ActiveInd { get; set; }
        public bool DeleteRecord { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string OpportunityDescription { get; set; }
        public string ExternalId { get; set; }
        public string OpportunityShortText1 { get; set; }
        public string OpportunityShortText2 { get; set; }
        public string OpportunityShortText3 { get; set; }
        public string OpportunityShortText4 { get; set; }
        public string OpportunityShortText5 { get; set; }
        public int? OpportunityNumber1 { get; set; }
        public int? OpportunityNumber2 { get; set; }
        public int? OpportunityNumber3 { get; set; }
        public int? OpportunityNumber4 { get; set; }
        public int? OpportunityNumber5 { get; set; }
        public DateTime? OpportunityDate1 { get; set; }
        public DateTime? OpportunityDate2 { get; set; }
        public DateTime? OpportunityDate3 { get; set; }
        public DateTime? OpportunityDate4 { get; set; }
        public DateTime? OpportunityDate5 { get; set; }
        public int? OpportunityValueListID1 { get; set; }
        public int? OpportunityValueListID2 { get; set; }
        public int? OpportunityValueListID3 { get; set; }
        public int? OpportunityValueListID4 { get; set; }
        public int? OpportunityValueListID5 { get; set; }
        public float? OpportunityMoney1 { get; set; }
        public float? OpportunityMoney2 { get; set; }
        public float? OpportunityMoney3 { get; set; }
        public float? OpportunityMoney4 { get; set; }
        public float? OpportunityMoney5 { get; set; }
        public float? OpportunityPercent1 { get; set; }
        public float? OpportunityPercent2 { get; set; }
        public float? OpportunityPercent3 { get; set; }
        public float? OpportunityPercent4 { get; set; }
        public float? OpportunityPercent5 { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public float? FirmFee { get; set; }
        public int? ProjectProbability { get; set; }
        public int? OwnerId { get; set; }
        public int? TeamOppFirmOrgFeeSource { get; set; }
        public string OppType { get; set; }
        public DateTime? PreConStartDate { get; set; }
        public DateTime? PreConEndDate { get; set; }
        public DateTime? DesignStartDate { get; set; }
        public DateTime? DesignCompletionDate { get; set; }
        public DateTime? ConstructionStartDate { get; set; }
        public DateTime? ConstructionCompletionDate { get; set; }
        public float? fundProbability { get; set; }
        public bool ShortListed { get; set; }
        public bool Interviewed { get; set; }
        public int? Score { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastDeletedDateTime { get; set; }
        public int? LastDeletedByUserId { get; set; }
        public string EstimatedFeePercentage { get; set; }
        public float? FactoredFee { get; set; }
        public float? GrossRevenueSTD { get; set; }
        public float? GrossMarginDollarsSTD { get; set; }
        public float? GrossMarginPercentSTD { get; set; }
        public float? FactoredCostSTD { get; set; }
        public float? FeeCIPercent { get; set; }
        public float? FeeCIDifVolume { get; set; }
        public int? LaborDifferentialDollars { get; set; }
        public int? OppSelfPerformHours { get; set; }
        public float? EstimatedManagementUnits { get; set; }
        public float? GrossMargin { get; set; }
        public float? GrossMarginPercent { get; set; }
        public int? MasterOpportunityId { get; set; }
        public string OppTypeDescription { get; set; }
        public string Stage { get; set; }
        public string StageType { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SubmittalType { get; set; }
        public string Role { get; set; }
        public string Sf330ProfileCode { get; set; }
        public string servicefeebreakdown { get; set; }



    }
}
