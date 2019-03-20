using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class StageType
    {
        public int StageTypeID { get; set; }
        public string StageTypeName { get; set; }
    }

    public class Stage
    {
        public int StageID { get; set; }
        public string StageName { get; set; }
        public StageType StageType { get; set; }
    }
}
