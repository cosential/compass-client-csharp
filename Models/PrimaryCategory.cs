using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PrimaryCategory
    {
        public int? PrimaryCategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsIndustryStandard { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool IsDefault { get; set; }
        public string ExternalCategoryId { get; set; }
        public string ProjectTypeCode { get; set; }
    }
}