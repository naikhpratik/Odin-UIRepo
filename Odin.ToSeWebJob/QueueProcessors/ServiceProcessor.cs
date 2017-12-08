using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ToSeWebJob.Domain;
using Odin.ToSeWebJob.Helpers;
using Odin.ToSeWebJob.Interfaces;
using ServicEngineImporter.Models;

namespace Odin.ToSeWebJob.QueueProcessors
{
    public class ServiceProcessor : IServiceProcessor
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProcessor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ProcessService(string serviceId)
        {
            var service = _unitOfWork.Services.GetServiceById(serviceId);
            var endpoint = EndPointHelper.GetEndPointForService(service);

            var url = ConfigHelper.GetBaldrApiUrl() + endpoint;
     
            var jsonData = ServiceToJson.GetJsonDataForService(service);

            var request = RequestHelper.CreateBaldrRequest(ConfigHelper.GetSeApiToken(), url, jsonData);

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
