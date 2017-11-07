using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Odin.Data.Core;
using Odin.Data.Persistence;

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
        public async Task ProcessQueueMessage([ServiceBusTrigger("odintose")] string message, TextWriter log)
        {
            var order = _unitOfWork.Orders.GetOrderByTrackingId("41");
            log.WriteLine(order.DestinationState);
        }

        
    }
}
