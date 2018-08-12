namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectConstructionCost
    {
        public int? ProjectID { get; set; }
        public decimal? OurResponsibility_AC { get; set; }
        public decimal? OurResponsibility_PC { get; set; }
        public decimal? OrigPreconst_AC { get; set; }
        public decimal? ConstBudget_AC { get; set; }
        public decimal? OwnTotalDevCost_AC { get; set; }
        public decimal? OrigConstContract_AC { get; set; }
        public decimal? OrigPreconst_PC { get; set; }
        public decimal? ModPreconst_PC { get; set; }
        public decimal? ConstBudget_PC { get; set; }
        public decimal? OwnTotalDevCost_PC { get; set; }
        public decimal? OrigConstContract_PC { get; set; }
        public decimal? ModConstContract_PC { get; set; }
        public decimal? StdChangeOrd_PC { get; set; }
        public int? GMPChangeOrd_count { get; set; }
        public decimal? GMPChangeOrd_val { get; set; }
        public int? PreconChangeOrd_count { get; set; }
        public decimal? PreconChangeOrd_val { get; set; }
        public int? StdChangeOrd_count { get; set; }
        public decimal? StdChangeOrd_val { get; set; }
        public string ChangeOrdComments { get; set; }
        public string ConstCostComments { get; set; }
        public decimal? FinCont_PC { get; set; }
        public float? FinCostab_PC { get; set; }
        public decimal? RFIs { get; set; }
        public decimal? ConsValueEngSav { get; set; }
        public float? ConsValueEngSavPercentage { get; set; }
        public decimal? TotalSavReturnedToOwner { get; set; }
        public bool ConfidentialCost { get; set; }
        public decimal? ModPreconst_AC { get; set; }
        public decimal? ModConstContract_AC { get; set; }
        public decimal? StdChangeOrd_AC { get; set; }
        public decimal? FinCont_AC { get; set; }
        public float? FinCostab_AC { get; set; }
        public int? TotalChangeOrd_count { get; set; }
        public decimal? TotalChangeOrd_val { get; set; }
        public string PreconInContract { get; set; }
    }
}