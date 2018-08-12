using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectInvoice
    {
        public int? Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public float? BilledAmount { get; set; }
        public DateTime? BilledDate { get; set; }
        public DateTime? DueDate { get; set; }
        public float? PaidAmount { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
    }
}