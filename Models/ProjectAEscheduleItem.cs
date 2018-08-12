using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectAEScheduleItem
    {
        public int? ScheduleItemID { get; set; }
        public int? ProjectID { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public bool IsCurrentPhase { get; set; }
        public string SchedulePhaseName { get; set; }
        public float? EstimatedVal { get; set; }
        public float? ActualVal { get; set; }
        public string PhaseComments { get; set; }
    }
}