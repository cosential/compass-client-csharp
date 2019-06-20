using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class OpportunityCompetitor
    {
        public int CompetitionID { get; set; }
        public int OpportunityID { get; set; }
        public int CompanyID { get; set; }
        public string Company { get; set; }
        public int FirmId { get; set; }
        public int ThreatLevel { get; set; }
        public int StatusID { get; set; }
        public string Comments { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string StatusName { get; set; }
        public Nullable<decimal> bid { get; set; }
        public Nullable<int> rank { get; set; }
        public Nullable<decimal> adjScore { get; set; }
        public Nullable<decimal> technicalScore { get; set; }
        public Nullable<decimal> bidPercent { get; set; }
        public bool DeleteRecord { get; set; }
        public byte[] ROW_VERSION { get; set; }
    }
}
