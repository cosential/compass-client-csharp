using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectAESchedule
    {
        public int? ScheduleId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectStatusName { get; set; }
        public string Comments { get; set; }
        public DateTime? ConstructionCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}