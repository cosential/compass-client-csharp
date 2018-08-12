using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectLeed
    {
        public int? projectId { get; set; }
        public bool isLEED { get; set; }
        public DateTime? LeedAwardDate { get; set; }
        public int? LeedPoints { get; set; }
        public ProjectLeedStatus LeedStatus { get; set; }
        public ProjectLeedAward LeedAward { get; set; }
        public ProjectLeedProgram LeedProgram { get; set; }
        public ProjectLeedVersion LeedVersion { get; set; }
        public string LeedProjectComments { get; set; }
    }
}