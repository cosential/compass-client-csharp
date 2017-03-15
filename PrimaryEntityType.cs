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
                    break;
                case PrimaryEntityType.Company:
                    return "companies";
                    break;
                case PrimaryEntityType.Contact:
                    return "contact";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}