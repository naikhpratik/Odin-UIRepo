using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Photo : MobileTable
    {
        public Photo(string propertyId, string storageId, string photoUrl)
        {
            PropertyId = propertyId;
            StorageId = storageId;
            PhotoUrl = photoUrl;
        }

        public string PropertyId { get; private set; }
        public Property Property { get; set; }
        public string StorageId { get; private set; }
        public string PhotoUrl { get; private set; }
    }
}
