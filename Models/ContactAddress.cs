using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cosential.Integrations.Compass.Client.Models
{
    public enum ContactAddressType
    {
        Home,
        Office
    }

    public class ContactAddress
    {
        public int AddressID { get; set; }
        public int ContactId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactAddressType AddressType { get; set; }
        public bool DefaultInd { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Comments { get; set; }
        public bool DeleteRecord { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public string Pager { get; set; }
        public string Other { get; set; }
        public string County { get; set; }
        public string PhoneExt { get; set; }
        public string ROW_VERSION { get; set; }
    }
}
