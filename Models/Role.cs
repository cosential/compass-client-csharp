using Cosential.Integrations.Compass.Client.Attributes;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models.Interfaces;

namespace Cosential.Integrations.Compass.Client.Models
{
    [CompassPath(schema: "opportunities/role/schema",
        get: "opportunities/role/{id}",
        getMany: "opportunities/role",
        createMany: "opportunities/role",
        delete: "opportunities/role/{id}",
        update: "opportunities/role/{id}")]
    public class Role : IValueList
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool AvailableRole { get; set; }
        public string PrimaryKeyName => nameof(RoleId);
        public object PrimaryKey
        {
            get => RoleId;
            set => RoleId = (int) value;
        }
        public bool PrimaryIsDeleted => false;
    }
}