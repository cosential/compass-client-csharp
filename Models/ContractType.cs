using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ContractType
    {
        public int? ContractTypeID { get; set; }
        public string ContractTypeName { get; set; }
        public string Description { get; set; }
        public int AvailableType { get; set; }
        public int? ContractTypeOrder { get; set; }
        public int DeleteRecord { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Old_ID { get; set; }
    }
}
