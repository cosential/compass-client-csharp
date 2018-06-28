using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelResumeIntroduction
    {
        public int? ResumeIntroductionId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Introduction { get; set; }
    }
}