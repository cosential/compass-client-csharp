using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectConstructionSchedule
    {
        public int? ProjectId { get; set; }
        public DateTime? DateOfBid { get; set; }
        public DateTime? DateOfAward { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? ScheduledCompletionDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public DateTime? ScheduledStartDateConstruction { get; set; }
        public DateTime? ScheduledSubstantialCompletionDate { get; set; }
        public DateTime? ActualStartDateConstruction { get; set; }
        public DateTime? ModifiedScheduledSubstantialCompletionDate { get; set; }
        public DateTime? ActualSubstantialCompletionDate { get; set; }
        public DateTime? FinalPaymentDate { get; set; }
        public DateTime? MidTermFollowUpDate { get; set; }
        public DateTime? TenMonthFollowUpDate { get; set; }
        public DateTime? WarrantyExpirationDate { get; set; }
        public string ReasonForScheduleDelay { get; set; }
        public int? OriginalScheduleInDays { get; set; }
        public int? DaysAheadBehind { get; set; }
        public float? PercentAheadBehind { get; set; }
        public DateTime? CalculatedStartDate { get; set; }
        public DateTime? CalculatedCompletionDate { get; set; }
        public string ScheduleComments { get; set; }
    }
}