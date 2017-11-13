using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Odin.Data.Core.Models;

namespace Odin.Interfaces
{
    public interface IQueueStore
    {
        void Add(QueueEntry entry);
    }
}
