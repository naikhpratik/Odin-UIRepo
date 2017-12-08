using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Extensions;

namespace Odin.ToSeWebJob.Helpers
{
    public static class EndPointHelper
    {
        public static readonly string FirstContactEndPoint = "firstContact";
        public static readonly string DestinationChecklistEndPoint = "destinationChecklist";
        public static readonly string SettlingInEndpoint = "settlingIn";
        public static readonly string AreaOrientationEndPoint = "areaOrientation";

        public static string GetEndPointForService(Service service)
        {
            if (service.ServiceType.Category == ServiceCategory.InitialConsultation ||
                service.ServiceType.Category == ServiceCategory.WelcomePacket)
                return FirstContactEndPoint;
            else if (service.Order.ProgramName.NullContains("Bundle") &&
                     ((int) service.ServiceType.Category & 12) > 0)
                return DestinationChecklistEndPoint;
            else if (service.ServiceType.Category == ServiceCategory.SettlingIn)
                return SettlingInEndpoint;
            else if (service.ServiceType.Category == ServiceCategory.AreaOrientation)
                return AreaOrientationEndPoint;

            return string.Empty;
        }
    }
}
