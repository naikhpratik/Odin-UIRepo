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

namespace Odin.PropBotWebJob
{
    public class Functions
    {
        private readonly IUnitOfWork _unitOfWork;

        public Functions(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called propbotqueue.
        public static void ProcessQueueMessage([QueueTrigger("propbotqueue")] string message, TextWriter log)
        {
            log.WriteLine(message);
            var queueEntry = JsonConvert.DeserializeObject<PropBotJobQueueEntry>(message);
            log.WriteLine(queueEntry.Notes);
        }
    }
}
