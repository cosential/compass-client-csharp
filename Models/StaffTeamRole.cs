using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class StaffTeamRole
    {

        public int StaffRoleId { get; set; }
        public string StaffRoleName { get; set; }
        public string StaffRoleType { get; set; }
        public string Description { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public int? ExternalId { get; set; }
    }
}
