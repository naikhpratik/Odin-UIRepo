using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Extensions;
using Odin.Data.Persistence;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob
{
    public class Functions
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxQueueCount = 5;

        public Functions(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        // Test throwing exception and make sure item does not get dequeued.
        // TODO: Add conversion library of odin to servicengineimporter objects 

        public async Task ProcessQueueMessage([QueueTrigger("odintose")] string message, int dequeueCount, TextWriter log)
        {
            log.WriteLine(message);
            if (dequeueCount >= MaxQueueCount)
            {
                log.WriteLine("Hit Max dequeue count");
                return;
            }
            
            var queueEntry = JsonConvert.DeserializeObject<OdinToSeQueueEntry>(message);
            if (queueEntry.QueueTypeId == (int) QueueType.Service)
            {
                //TODO: Put process service in separate classs
                var service = _unitOfWork.Services.GetServiceById(queueEntry.ObjectId);
                var endPoint = GetEndPointForService(service);
                var json = GetJsonForService(service);

                //TODO: Environment variables
                var environmentUrl = "https://localhost:44372/api/";
                var apiKey = "VtADQse3uRTspdtGjSgp";

                //TODO: Make Request/Send Helper
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{environmentUrl}{endPoint}"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Token", apiKey);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request);
                    log.WriteLine($"RESPOSNE: {response}");
                }
            }
        }

        public string GetEndPointForService(Service service)
        {
            if (service.ServiceType.Category == ServiceCategory.InitialConsultation || service.ServiceType.Category == ServiceCategory.WelcomePacket)
                return "firstContact";
            else if (service.Order.ProgramName.NullContains("Bundled") && ((int) service.ServiceType.Category & 12) == 0)
                return "destinationChecklist";
            else if (service.ServiceType.Category == ServiceCategory.SettlingIn)
                return "settlingIn";
            else if (service.ServiceType.Category == ServiceCategory.AreaOrientation)
                return "areaOrientation";

            return string.Empty;
        }

        public string GetColumnNameForServiceType(ServiceType serviceType)
        {
            var columnName = string.Empty;

            return columnName;
        }

        public string GetJsonForService(Service service)
        {
            var json = string.Empty;

            var serviceEngineId = Convert.ToInt32(service.Order.TrackingId);
            var firstContact = new FirstContact(serviceEngineId);
            switch (service.ServiceTypeId)
            {
                case 1:
                    firstContact.FirstFaceToFaceMeetingDate = service.CompletedDate;
                    json = JsonConvert.SerializeObject(firstContact);
                    break;
                case 2:
                    firstContact.EstimatedFirstMeetingDate = service.CompletedDate;
                    json = JsonConvert.SerializeObject(firstContact);
                    break;
            }

            return json;
        }
    }
}
