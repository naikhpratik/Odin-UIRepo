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
    public static class SettlingInConverter
    {
        public static string GetJson(Service service)
        {
            var serviceEngineId = Convert.ToInt32(service.Order.TrackingId);
            var settlingIn = new SettlingIn(serviceEngineId);
            switch (service.ServiceTypeId)
            {
                case 3:
                    settlingIn.SocialSecurityRegistration = service.CompletedDate;
                    break;
                case 4:
                    settlingIn.CollegeUniversities = service.CompletedDate;
                    break;
                case 5:
                    settlingIn.LanguageAssistance = service.CompletedDate;
                    break;
                case 6:
                    settlingIn.TelephoneSystems = service.CompletedDate;
                    break;
                case 7:
                    settlingIn.UtilityHookUp = service.CompletedDate;
                    break;
                case 8:
                    settlingIn.InternetServiceProviders = service.CompletedDate;
                    break;
                case 9:
                    settlingIn.FurnitureRental = service.CompletedDate;
                    break;
                case 10:
                    settlingIn.FurniturePurchase = service.CompletedDate;
                    break;
                case 11:
                    settlingIn.Appliances = service.CompletedDate;
                    break;
                case 12:
                    settlingIn.DrivingAutoInfo = service.CompletedDate;
                    break;
                case 13:
                    settlingIn.TransportationOptions = service.CompletedDate;
                    break;
                case 15:
                    settlingIn.Insurance = service.CompletedDate;
                    break;
                case 16:
                    settlingIn.MedicalDentalInformation = service.CompletedDate;
                    break;
                case 17:
                    settlingIn.MoneyIssuesBanking = service.CompletedDate;
                    break;
                case 18:
                    settlingIn.MailServices = service.CompletedDate;
                    break;
                case 19:
                    settlingIn.DomesticServices = service.CompletedDate;
                    break;
                case 20:
                    settlingIn.Pets = service.CompletedDate;
                    break;
                case 21:
                    settlingIn.Childcare = service.CompletedDate;
                    break;
                case 22:
                    settlingIn.Eldercare = service.CompletedDate;
                    break;

            }
            return JsonConvert.SerializeObject(settlingIn);
        }
    }
}
