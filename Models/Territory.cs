using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Territory
    {
        public int? TerritoryID { get; set; }
        public string TerritoryName { get; set; }
        public string zipCodes { get; set; }
        public DateTime? createDate { get; set; }
        public bool deleterecord { get; set; }
        public string states { get; set; }
        public string FIPSCodes { get; set; }
        public bool available { get; set; }
        public string ROW_VERSION { get; set; }
    }
}
