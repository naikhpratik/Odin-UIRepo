using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Odin.Data.Core.Models;
using Odin.Data.Extensions;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob.Domain
{
    public static class ServiceToJson
    {
        public static string GetJsonDataForService(Service service)
        {
            if (service.ServiceType.Category == ServiceCategory.InitialConsultation ||
                service.ServiceType.Category == ServiceCategory.WelcomePacket)
                return FirstContactConverter.GetJson(service);
            else if (service.Order.ProgramName.NullContains("Bundle") &&
                     ((int)service.ServiceType.Category & 12) > 0)
                return DestinationChecklistConverter.GetJson(service);
            else if (service.ServiceType.Category == ServiceCategory.SettlingIn)
                return SettlingInConverter.GetJson(service);
            else if (service.ServiceType.Category == ServiceCategory.AreaOrientation)
                return AreaOrientationConverter.GetJson(service);

            return string.Empty;
        }



    }
}
