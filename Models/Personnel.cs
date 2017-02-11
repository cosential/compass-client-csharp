using System;
using RestSharp;

namespace Cosential.Integrations.CompassApiClient.Models
{
    public class Personnel
    {
        public Personnel()
        {
            //defaults
            Status = "Active";
        }

        public int? PersonnelId { get; set; } //Cosential's primary key
        public string ExternalId { get; set; } //Customer assigned employee number
        public string FirstName { get; set; }
        public string MI { get; set; } //Middle name, not middle initial
        public string LastName { get; set; }
        public string Status { get; set; }
        public int? SupervisorId { get; set; } //Cosential's PersonnelId for the supervisor
        public string Title { get; set; }
        public DateTime? StartDate { get; set; } //Hire date
        public DateTime? EndDate { get; set; } //Termination date
        public string OfficeEmail { get; set; }
        public string OfficePhone { get; set; }
        public string OfficePhoneExtension { get; set; }
        public string NickName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
