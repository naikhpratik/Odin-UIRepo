using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Odin.Data.Core.Models;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob.Domain
{
    public static class AreaOrientationConverter
    {
        public static string GetJson(Service service)
        {
            var servicEngineId = Convert.ToInt32(service.Order.TrackingId);
            var areaOrientation = new AreaOrientation(servicEngineId);
            switch (service.ServiceTypeId)
            {
                case 24:
                    areaOrientation.HousingNeighborhoods = service.CompletedDate;
                    break;
                case 25:
                    areaOrientation.ReligiousWorship = service.CompletedDate;
                    break;
                case 26:
                    areaOrientation.ShoppingInformation = service.CompletedDate;
                    break;
                case 27:
                    areaOrientation.Restaurants = service.CompletedDate;
                    break;
                case 28:
                    areaOrientation.ArtsAndLeisureFacilities = service.CompletedDate;
                    break;
                case 29:
                    areaOrientation.Clubs = service.CompletedDate;
                    break;
                case 30:
                    areaOrientation.SportsInformation = service.CompletedDate;
                    break;
                case 31:
                    areaOrientation.VolunteerAssociations = service.CompletedDate;
                    break;
                case 32:
                    areaOrientation.Library = service.CompletedDate;
                    break;
                case 33:
                    areaOrientation.EmergencyPoliceFire = service.CompletedDate;
                    break;
                case 34:
                    areaOrientation.Holidays = service.CompletedDate;
                    break;
                case 35:
                    areaOrientation.AreaOrientationOverview = service.CompletedDate;
                    break;
            }
            return JsonConvert.SerializeObject(areaOrientation);
        }
    }
}
