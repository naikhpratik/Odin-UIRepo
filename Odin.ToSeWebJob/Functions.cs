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

        public async Task ProcessQueueMessage([QueueTrigger("odintose")] string message, TextWriter log)
        {
            log.WriteLine(message);
            var queueEntry = JsonConvert.DeserializeObject<OdinToSeQueueEntry>(message);
            log.WriteLine(queueEntry.ObjectId);
            log.WriteLine("From VSTS second deploy");
        }
    }
}
