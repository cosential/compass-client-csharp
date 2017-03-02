using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class AuthenticatedUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string FirmName { get; set; }
        public int UserId { get; set; }
        public Guid UserToken { get; set; }
    }
}
