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
    public static class FirstContactConverter
    {
        public static string GetJson(Service service)
        {
            var serviceEngineId = Convert.ToInt32(service.Order.TrackingId);
            var firstContact = new FirstContact(serviceEngineId);
            switch (service.ServiceTypeId)
            {
                case 1:
                    firstContact.FirstFaceToFaceMeetingDate = service.CompletedDate;
                    break;
                case 2:
                    firstContact.EstimatedFirstMeetingDate = service.CompletedDate;
                    break;
            }

            return JsonConvert.SerializeObject(firstContact);
        }
    }
}
