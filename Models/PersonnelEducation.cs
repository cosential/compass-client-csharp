namespace Cosential.Integrations.Compass.Client.Models
{
    public class PersonnelEducation
    {
        public int? DegreeId { get; set; }
        public string Major { get; set; }
        public string School { get; set; }
        public string Year { get; set; }
        public string DegreeType { get; set; }
        public string ExternalId { get; set; }
        public int? SF330_DegreeInd { get; set; }
        public int? DegreeTypeId { get; set; }
    }
}