using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class DeliveryMethod
    {
        public int DeliveryMethodID { get; set; }
        public string DeliveryMethodName { get; set; }
        public string Description { get; set; }
        public int AvailableType { get; set; }
        public int DeleteRecord { get; set; }
    }
}
