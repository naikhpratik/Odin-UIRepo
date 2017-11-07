using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class CloudQueueStore : ICloudQueueStore
    {
        private readonly CloudQueue _queue;

        public CloudQueueStore(CloudQueue queue)
        {
            _queue = queue;
        }

        public void Add(QueueEntry entry)
        {
            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(entry));
            _queue.AddMessage(message);
        }
        
    }
}