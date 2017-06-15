using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class SecondaryCategory
    {
        public int SecondaryCategoryID { get; set; }
        public string SecondaryCategoryName { get; set; }
        public bool IndustryStd { get; set; }
        public bool AvailableCat { get; set; }
        public bool DeleteCat { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool DefaultCat { get; set; }
        public string Old_CatID { get; set; }
    }
}
