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
    public static class DestinationChecklistConverter
    {
        public static string GetJson(Service service)
        {
            var servicEngineId = Convert.ToInt32(service.Order.TrackingId);
            var destinationChecklist = new DestinationChecklist(servicEngineId);
            switch (service.ServiceTypeId)
            {
                case 3:
                    destinationChecklist.SocialSecurityRegistration = service.CompletedDate;
                    break;
                case 4:
                    destinationChecklist.CollegeUniversities = service.CompletedDate;
                    break;
                case 5:
                    destinationChecklist.LanguageAssistance = service.CompletedDate;
                    break;
                case 6:
                    destinationChecklist.TelephoneSystems = service.CompletedDate;
                    break;
                case 7:
                    destinationChecklist.UtilityHookUp = service.CompletedDate;
                    break;
                case 8:
                    destinationChecklist.InternetServiceProviders = service.CompletedDate;
                    break;
                case 9:
                    destinationChecklist.FurnitureRental = service.CompletedDate;
                    break;
                case 10:
                    destinationChecklist.FurniturePurchase = service.CompletedDate;
                    break;
                case 11:
                    destinationChecklist.Appliances = service.CompletedDate;
                    break;
                case 12:
                    destinationChecklist.DrivingAutoInfo = service.CompletedDate;
                    break;
                case 13:
                    destinationChecklist.TransportationOptions = service.CompletedDate;
                    break;
                case 15:
                    destinationChecklist.Insurance = service.CompletedDate;
                    break;
                case 16:
                    destinationChecklist.MedicalDentalInformation = service.CompletedDate;
                    break;
                case 17:
                    destinationChecklist.MoneyIssuesBanking = service.CompletedDate;
                    break;
                case 18:
                    destinationChecklist.MailServices = service.CompletedDate;
                    break;
                case 19:
                    destinationChecklist.DomesticServices = service.CompletedDate;
                    break;
                case 20:
                    destinationChecklist.Pets = service.CompletedDate;
                    break;
                case 21:
                    destinationChecklist.Childcare = service.CompletedDate;
                    break;
                case 22:
                    destinationChecklist.Eldercare = service.CompletedDate;
                    break;
                case 24:
                    destinationChecklist.HousingNeighborhoods = service.CompletedDate;
                    break;
                case 25:
                    destinationChecklist.ReligiousWorship = service.CompletedDate;
                    break;
                case 26:
                    destinationChecklist.ShoppingInformation = service.CompletedDate;
                    break;
                case 27:
                    destinationChecklist.Restaurants = service.CompletedDate;
                    break;
                case 28:
                    destinationChecklist.ArtsAndLeisureFacilities = service.CompletedDate;
                    break;
                case 29:
                    destinationChecklist.Clubs = service.CompletedDate;
                    break;
                case 30:
                    destinationChecklist.SportsInformation = service.CompletedDate;
                    break;
                case 31:
                    destinationChecklist.VolunteerAssociations = service.CompletedDate;
                    break;
                case 32:
                    destinationChecklist.Library = service.CompletedDate;
                    break;
                case 33:
                    destinationChecklist.EmergencyPoliceFire = service.CompletedDate;
                    break;
                case 34:
                    destinationChecklist.Holidays = service.CompletedDate;
                    break;
                case 35:
                    destinationChecklist.AreaOrientationOverview = service.CompletedDate;
                    break;
            }
            return JsonConvert.SerializeObject(destinationChecklist);
        }
    }
}
