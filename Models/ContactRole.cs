namespace Cosential.Integrations.Compass.Client.Models
{
    public class ContactRole
    {
        public int? ContactRoleId { get; set; }
        public string ContactRoleName { get; set; }
        public bool IsDeleted { get; set; }
        public ContactRoleType ContactRoleType { get; set; }
    }
}