using System;

namespace Odin.Data.Core.Models
{
    public class QueueEntry
    {
        public QueueEntry(string objectId, QueueType queueType)
        {
            ObjectId = objectId;
            QueueTypeId = (int)queueType;
        }

        public string ObjectId { get; set; }
        public int QueueTypeId { get; set; }
    }
}
