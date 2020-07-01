using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public static class FirmOrgContext
    {

        internal static string GetPrimaryKeyName(FirmOrg firmorg)
        {
            switch (firmorg)
            {
                case FirmOrg.Offices:
                    return nameof(Office.OfficeID);
                case FirmOrg.Divisions:
                    return nameof(Division.DivisionID);
                case FirmOrg.Studios:
                    return nameof(Studio.StudioID);
                case FirmOrg.PracticeAreas:
                    return nameof(PracticeArea.PracticeAreaID);
                case FirmOrg.Territories:
                    return nameof(Territory.TerritoryID);
                default:
                    throw new ArgumentOutOfRangeException(nameof(firmorg), firmorg, null);
            }
        }
    }
}
