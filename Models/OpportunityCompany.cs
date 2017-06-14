using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class OpportunityCompany
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public CompanyIdent Company { get; set; }
        public ProjectRoleType RoleType { get; set; }
    }
}
