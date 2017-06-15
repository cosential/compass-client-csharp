namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelSummary
    {
        public int PersonnelId { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public string Acronym { get; set; }
        public string NickName { get; set; }
        public string FormalName { get; set; }
        public string OfficeCellPhone { get; set; }
        public string OfficePhone { get; set; }
        public string OfficePhoneExtension { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalCellPhone { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string HomeEmail { get; set; }
        public string ExternalId { get; set; }
        public PersonnelStatus Status { get; set; }
    }
}