using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class ProjectChangeOrder
    {
        public int? ChangeOrderId { get; set; }
        public string ChangeOrderNum { get; set; }
        public DateTime? ChangeOrderDate { get; set; }
        public string ChangeOrderAffect { get; set; }
        public float? Amount { get; set; }
        public float? FeeAmount { get; set; }
        public string Reason { get; set; }
        public float? OwnerRequestedAmount { get; set; }
        public string RequestedBy { get; set; }
        public string ChangeOrderTypeName { get; set; }
        public bool DeleteRecord { get; set; }
    }
}