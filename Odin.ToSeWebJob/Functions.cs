using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob
{
    public class Functions
    {
        private readonly IUnitOfWork _unitOfWork;

        public Functions(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        // Test throwing exception and make sure item does not get dequeued.
        // TODO: Add conversion library of odin to servicengineimporter objects 

        public async Task ProcessQueueMessage([QueueTrigger("odintose")] string message, TextWriter log)
        {
            log.WriteLine(message);
            var queueEntry = JsonConvert.DeserializeObject<OdinToSeQueueEntry>(message);
            log.WriteLine(queueEntry.ObjectId);
            log.WriteLine("From VSTS second deploy");
            if (queueEntry.QueueTypeId == (int) QueueType.Service)
            {
                var service = _unitOfWork.Services.GetServiceById(queueEntry.ObjectId);
                var endPoint = GetEndPointForService(service);
                var json = GetJsonForService(service);
                //TODO: Post JSON
            }
        }

        public string GetEndPointForService(Service service)
        {
            if (((int)service.ServiceType.Category & 3) == 0)
                return "firstContact";
            else if (service.Order.ProgramName.Contains("Bundled") && ((int) service.ServiceType.Category & 12) == 0)
                return "destinationChecklist";
            else if ((int) service.ServiceType.Category == 4)
                return "settlingIn";
            else if ((int) service.ServiceType.Category == 8)
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
            switch (service.ServiceTypeId)
            {
                case 1:
                    var firstContact = new FirstContact(serviceEngineId);
                    firstContact.FirstFaceToFaceMeetingDate = service.CompletedDate;
                    json = JsonConvert.SerializeObject(firstContact);
                    break;
            }

            return json;
        }
    }
}
