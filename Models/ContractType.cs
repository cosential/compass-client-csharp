using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ContractType
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int? ContractTypeOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Old_ID { get; set; }
        public string ROW_VERSION { get; set; }
    }
}