using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Attributes;
using Cosential.Integrations.Compass.Client.Contexts;
using Cosential.Integrations.Compass.Client.Models.Interfaces;

namespace Cosential.Integrations.Compass.Client.Models
{
    [CompassPath(schema: "opportunities/Sf330ProfileCode/schema", 
        get: "opportunities/Sf330ProfileCode/{id}", 
        getMany: "opportunities/Sf330ProfileCode",
        createMany: "opportunities/Sf330ProfileCode",
        delete: "opportunities/Sf330ProfileCode/{id}")]
    public class Sf330ProfileCode : IValueList
    {
        public int      ProfileCodeId   { get; set; }
        public string   ProfileCodeName { get; set; }
        public string   ProfileCodeNum  { get; set; }
        public bool     IsDefault       { get; set; }

        public string PrimaryKeyName => nameof(ProfileCodeId);

        public object PrimaryKey
        {
            get => ProfileCodeId;
            set => ProfileCodeId = (int) value;
        }

        public bool PrimaryIsDeleted => false;
    }
}
