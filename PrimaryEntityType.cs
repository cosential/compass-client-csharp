using System;

namespace Cosential.Integrations.Compass.Client
{
    public enum PrimaryEntityType
    {
        Personnel,
        Company,
        Contact    
    }

    public static class PrimaryEntityTypeExtensions
    {
        public static string ToPlural(this PrimaryEntityType type)
        {
            switch (type)
            {
                case PrimaryEntityType.Personnel:
                    return "personnel";
                case PrimaryEntityType.Company:
                    return "companies";
                case PrimaryEntityType.Contact:
                    return "contacts";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}