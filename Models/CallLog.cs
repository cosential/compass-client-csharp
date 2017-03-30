using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class CallLog
    {

        public CallLog() { }

        public int? id { get; set; }
        public int? firmId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string CallType { get; set; }
        public string subject { get; set; }
        public string comments { get; set; }
        public bool isFollowup { get; set; }
        public string CallDisposition { get; set; }
        public string status { get; set; }
        public bool deleteRecord { get; set; }
        public string CreatedBy { get; set; }
        public int? CreatedByUserId { get; set; }
        public string CreateDateTime { get; set; }

    }
}
