using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Attributes;
using Cosential.Integrations.Compass.Client.Models.Interfaces;

namespace Cosential.Integrations.Compass.Client.Models
{
    [CompassPath(schema: "opportunities/clienttypes/schema",
        get: "opportunities/clienttypes/{id}",
        getMany: "opportunities/clienttypes",
        createMany: "opportunities/clienttypes",
        delete: "opportunities/clienttypes/{id}",
        update: "opportunities/clienttypes/{id}")]
    public class ClientType : IValueList
    {
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string PrimaryKeyName => "Id";

        public object PrimaryKey
        {
            get => Id;
            set => Id = (int)value;
        }

        public bool PrimaryIsDeleted => IsDeleted;
    }
}
