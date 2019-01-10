using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class CompanyAddress
    {
        public int AddressID { get; set; }
        public string AddressTypeName { get; set; }
        public bool DefaultInd { get; set; }
        public int CompanyId { get; set; }
        public DateTime? createdate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StateAbrv { get; set; }
        public string CountryName { get; set; }
        public string Comments { get; set; }
        public bool deleterecord { get; set; } 
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public bool IsAddressVerified { get; set; }
        public DateTime? AddrVerifiedDateTime { get; set; }
        public string AddrVerifiedMethod { get; set; }
        public float? AddrLat { get; set; }
        public float? AddrLong { get; set; }
        public string County { get; set; }
        public string CongressDistrict { get; set; }
        public string CarrierRoute { get; set; }
        public string OfficeSecPhone { get; set; }
        public string ROW_VERSION { get; set; }

    }
}
