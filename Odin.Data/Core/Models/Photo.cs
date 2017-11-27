using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Photo : MobileTable
    {
        public Photo(string storageId, string photoUrl)
        {
            StorageId = storageId;
            PhotoUrl = photoUrl;
        }

        public Property Property { get; set; }
        public string StorageId { get; private set; }
        public string PhotoUrl { get; private set; }
    }
}
