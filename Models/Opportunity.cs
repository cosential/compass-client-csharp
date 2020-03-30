using System;
using System.Collections.Generic;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Opportunity
    {

        public Opportunity()
        {
            this.Offices = new HashSet<Office>();
           
        }


        public int? OpportunityId { get; set; }
        public int? ClientId { get; set; }
        public int? SF255Form { get; set; }
        public int? SF330Form { get; set; }
        public string OpportunityName { get; set; }
        public DateTime? EstimatedSelectionDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? Cost { get; set; }
        public int? Size { get; set; }
        public string SizeUnit { get; set; }
        public decimal? Fee { get; set; }
        public int? Probability { get; set; }
        public string NextAction { get; set; }
        public bool? RFPRecieved { get; set; }
        public string ProposalNumber { get; set; }
        public DateTime? ProposalDueDate { get; set; }
        public DateTime? ExpectedRFPDate { get; set; }
        public DateTime? QualsDueDate { get; set; }
        public bool? ProposalSubmitted { get; set; }
        public DateTime? PresentationDate { get; set; }
        public string Comments { get; set; }
        public decimal? MarketingCostBudget { get; set; }
        public decimal? MarketingCostActual { get; set; }
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
        public decimal? OpportunityMoney1 { get; set; }
        public decimal? OpportunityMoney2 { get; set; }
        public decimal? OpportunityMoney3 { get; set; }
        public decimal? OpportunityMoney4 { get; set; }
        public decimal? OpportunityMoney5 { get; set; }
        public decimal? OpportunityPercent1 { get; set; }
        public decimal? OpportunityPercent2 { get; set; }
        public decimal? OpportunityPercent3 { get; set; }
        public decimal? OpportunityPercent4 { get; set; }
        public decimal? OpportunityPercent5 { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public decimal? FirmFee { get; set; }
        public int? ProjectProbability { get; set; }
        public int? OwnerId { get; set; }
        public int? TeamOppFirmOrgFeeSource { get; set; }
        public byte OppType { get; set; }
        public DateTime? PreConStartDate { get; set; }
        public DateTime? PreConEndDate { get; set; }
        public DateTime? DesignStartDate { get; set; }
        public DateTime? DesignCompletionDate { get; set; }
        public DateTime? ConstructionStartDate { get; set; }
        public DateTime? ConstructionCompletionDate { get; set; }
        public decimal? fundProbability { get; set; }
        public bool? ShortListed { get; set; }
        public bool? Interviewed { get; set; }
        public int? Score { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastDeletedDateTime { get; set; }
        public int? LastDeletedByUserId { get; set; }
        public string EstimatedFeePercentage { get; set; }
        public decimal? FactoredFee { get; set; }
        public decimal? GrossRevenueSTD { get; set; }
        public decimal? GrossMarginDollarsSTD { get; set; }
        public decimal? GrossMarginPercentSTD { get; set; }
        public decimal? FactoredCostSTD { get; set; }
        public decimal? FeeCIPercent { get; set; }
        public decimal? FeeCIDifVolume { get; set; }
        public int? LaborDifferentialDollars { get; set; }
        public int? OppSelfPerformHours { get; set; }
        public float? EstimatedManagementUnits { get; set; }
        public decimal? GrossMargin { get; set; }
        public decimal? GrossMarginPercent { get; set; }
        public int? MasterOpportunityId { get; set; }
        public string OppTypeDescription { get; set; }
        public int? StageId { get; set; }
        public string Stage { get; set; }
        public string StageType { get; set; }
        public int? AutoNumber { get; set; }
        public DateTime? ShortListedDate { get; set; }
        public string Note { get; set; }
        public int? RedZoneScore { get; set; }
        public string OpportunityLongText1 { get; set; }
        public string OpportunityLongText2 { get; set; }
        public string OpportunityLongText3 { get; set; }
        public string OpportunityLongText4 { get; set; }
        public string OpportunityLongText5 { get; set; }
        public string approvalLevel { get; set; }
        public string approvalStatus { get; set; }
        public virtual ICollection<Office> Offices { get; set; }

    }
}