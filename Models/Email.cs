using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Email
    {
        public int? SchemaVersion { get; set; }
        public string EmailId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? Sent { get; set; }
        public bool HasAttachment { get; set; }
    }
}