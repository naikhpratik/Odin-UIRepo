using System;

namespace Odin.Data.Core.Dtos
{
    public class MessageDto
    {
        public string Id { get; set; }

        public string HomeFindingPropertyId { get; set; }

        public bool Deleted { get; set; }

        public string MessageText { get; set; }

        public DateTime? MessageDate { get; set; }
        
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public bool IsRead { get; set; }

    }
}
