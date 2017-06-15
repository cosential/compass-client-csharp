using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffTeamRole
    {

        public int? StaffTeamRoleId { get; set; }
        public string StaffRoleName { get; set; }
        public string StaffRoleType { get; set; }
        public string Description { get; set; }
        public float? HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }

    }
}
