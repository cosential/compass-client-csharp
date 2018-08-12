using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectDescriptionType
    {
        public int? DescriptionTypeID { get; set; }
        public string DescriptionTypeName { get; set; }
        public bool DeleteRecord { get; set; }
        public bool AvailableType { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool DefaultType { get; set; }
        public string old_desctypeid { get; set; }
        public bool IndustryStd { get; set; }
    }
}