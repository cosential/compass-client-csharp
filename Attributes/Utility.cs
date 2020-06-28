using System;
using System.Linq;

namespace Cosential.Integrations.Compass.Client.Attributes
{
    public static class Utility
    {
        public static CompassPathAttribute GetCompassPaths(this Type type) 
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttributes(typeof(CompassPathAttribute), true)
                .FirstOrDefault() as CompassPathAttribute;
        }
    }
}