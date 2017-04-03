using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Company
    {

        public Company() {

        }


        public int? CompanyId { get; set; } // A company's primary key.
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Website { get; set; }
        public string ExternalId { get; set; }
        public string FPISImportId { get; set; }
        public string ParentCompanyName { get; set; }
        public string TickerSymbol { get; set; }
        public float? SalesTarget { get; set; }
        public string AnnualRevenue { get; set; }
        public int? NumberOfEmployees { get; set; }
        public bool isHeadquarters { get; set; }
        public string ContactFirmsShortText1 { get; set; }
        public string ContactFirmsShortText2 { get; set; }
        public string ContactFirmsShortText3 { get; set; }
        public string ContactFirmsShortText4 { get; set; }
        public string ContactFirmsShortText5 { get; set; }
        public string ContactFirmsLongText1 { get; set; }
        public string ContactFirmsLongText2 { get; set; }
        public string ContactFirmsLongText3 { get; set; }
        public string ContactFirmsLongText4 { get; set; }
        public string ContactFirmsLongText5 { get; set; }
        public string ContactFirmsNumber1 { get; set; }
        public string ContactFirmsNumber2 { get; set; }
        public string ContactFirmsNumber3 { get; set; }
        public string ContactFirmsNumber4 { get; set; }
        public string ContactFirmsNumber5 { get; set; }
        public float? ContactFirmsMoney1 { get; set; }
        public float? ContactFirmsMoney2 { get; set; }
        public float? ContactFirmsMoney3 { get; set; }
        public float? ContactFirmsMoney4 { get; set; }
        public float? ContactFirmsMoney5 { get; set; }
        public float? ContactFirmsPercent1 { get; set; }
        public float? ContactFirmsPercent2 { get; set; }
        public float? ContactFirmsPercent3 { get; set; }
        public float? ContactFirmsPercent4 { get; set; }
        public float? ContactFirmsPercent5 { get; set; }
        public string OtherCompanyName { get; set; }
        public string Division { get; set; }
        public string Notes { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public DateTime? LastDeletedDateTime { get; set; }
        public int? LastDeletedByUserId { get; set; }
        public int? ParentCompanyId { get; set; }
        public string Services { get; set; }
        public string Sector { get; set; }
        public byte[] ROW_VERSION { get; set; }
    }
}
