using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Contact
    {

        public Contact()
        {

        }

        public int? ContactId { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public string FormalName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public bool IsPrimaryContact { get; set; }
        public int IsPrivate { get; set; }
        public int IsActive { get; set; }
        public bool DeleteRecord { get; set; }
        public string HomeEMailAddress { get; set; }
        public string BusinessEmailAddress { get; set; }
        public string AssistantName { get; set; }
        public string AssistantPhone { get; set; }
        public string AssistantEmail { get; set; }
        public string Notes { get; set; }
        public string Department { get; set; }
        public DateTime Birthday { get; set; }
        public string SpouseName { get; set; }
        public DateTime SpouseBirthday { get; set; }
        public string ExternalId { get; set; }
        public int ImportedRecord { get; set; }
        public int SameAsCompanyId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public int LastModifiedByUserId { get; set; }
        public DateTime LastDeletedDateTime { get; set; }
        public int LastDeletedByUserId { get; set; }

    }
}
