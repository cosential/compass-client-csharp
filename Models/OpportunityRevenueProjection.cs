using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class OpportunityRevenueProjection
    {
        public int RevenueId { get; set; }
        public int OpportunityId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Value { get; set; }
        public byte[] ROW_VERSION { get; set; }
    }
}
