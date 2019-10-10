using Cosential.Integrations.Compass.Client.Attributes;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models.Interfaces;

namespace Cosential.Integrations.Compass.Client.Models
{
    [CompassPath(schema: "opportunities/SubmittalType/schema",
        get: "opportunities/SubmittalType/{id}",
        getMany: "opportunities/SubmittalType")]
    public class SubmittalType : IValueList
    {
        public int SubmittalTypeId { get; set; }
        public string SubmittalTypeName { get; set; }
        public string PrimaryKeyName => nameof(SubmittalTypeId);
        public object PrimaryKey
        {
            get => SubmittalTypeId;
            set => SubmittalTypeId = (int) value;
        }
        public bool PrimaryIsDeleted => false;
    }
}