using System;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelSocial
    {
        public int? Id { get; set; }
        public int? SocialNetworkId { get; set; }
        public string SocialNetworkName { get; set; }
        public string Url { get; set; }
    }
}