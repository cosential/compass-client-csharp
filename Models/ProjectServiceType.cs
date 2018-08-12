using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectServiceType
    {
        public int? ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public bool DeleteType { get; set; }
        public bool AvailableType { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool DefaultType { get; set; }
        public string old_servicetypeid { get; set; }
        public bool deleterecord { get; set; }
        public bool defaultInd { get; set; }
    }
}