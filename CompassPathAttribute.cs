using System;
using Cosential.Integrations.Compass.Client.Contexts;

namespace Cosential.Integrations.Compass.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class CompassPathAttribute : Attribute
    {
        public string Get { get; }
        public string GetMany { get; }
        public string Create { get; }
        public string CreateMany { get; }
        public string Update { get; }
        public string UpdateMany { get; }
        public string Delete { get; }
        public string DeleteMany { get; }
        public string Schema { get; }
        public string Changes { get; }

        public CompassPathAttribute(string get = null, string getMany = null, string create = null
            , string createMany = null, string update = null, string updateMany = null, string delete = null,
            string deleteMany = null, string schema = null, string changes = null)
        {
            Get = get;
            GetMany = getMany;
            Create = create;
            CreateMany = createMany;
            Update = update;
            UpdateMany = updateMany;
            Delete = delete;
            DeleteMany = deleteMany;
            Schema = schema;
            Changes = changes;
        }

        public bool HasEndpoint(EndpointType endpointType)
        {
            switch (endpointType)
            {
                case EndpointType.Get:
                    return !string.IsNullOrWhiteSpace(Get);
                case EndpointType.GetMany:
                    return !string.IsNullOrWhiteSpace(GetMany);
                case EndpointType.Create:
                    return !string.IsNullOrWhiteSpace(Create);
                case EndpointType.CreateMany:
                    return !string.IsNullOrWhiteSpace(CreateMany);
                case EndpointType.Update:
                    return !string.IsNullOrWhiteSpace(Update);
                case EndpointType.UpdateMany:
                    return !string.IsNullOrWhiteSpace(UpdateMany);
                case EndpointType.Delete:
                    return !string.IsNullOrWhiteSpace(Delete);
                case EndpointType.DeleteMany:
                    return !string.IsNullOrWhiteSpace(DeleteMany);
                case EndpointType.Schema:
                    return !string.IsNullOrWhiteSpace(Schema);
                case EndpointType.Changes:
                    return !string.IsNullOrWhiteSpace(Changes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(endpointType), endpointType, null);
            }
        }
        public string Endpoint(EndpointType endpointType)
        {
            switch (endpointType)
            {
                case EndpointType.Get:
                    return Get;
                case EndpointType.GetMany:
                    return GetMany;
                case EndpointType.Create:
                    return Create;
                case EndpointType.CreateMany:
                    return CreateMany;
                case EndpointType.Update:
                    return Update;
                case EndpointType.UpdateMany:
                    return UpdateMany;
                case EndpointType.Delete:
                    return Delete;
                case EndpointType.DeleteMany:
                    return DeleteMany;
                case EndpointType.Schema:
                    return Schema;
                case EndpointType.Changes:
                    return Changes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endpointType), endpointType, null);
            }
        }
    }
}