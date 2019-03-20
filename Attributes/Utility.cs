using System;
using System.Linq;

namespace Cosential.Integrations.Compass.Client.Attributes
{
    public static class Utility
    {
        public static CompassPathAttribute GetCompassPaths(this Type t) 
        {
            return t.GetCustomAttributes(typeof(CompassPathAttribute), true)
                .FirstOrDefault() as CompassPathAttribute;
        }
    }
}