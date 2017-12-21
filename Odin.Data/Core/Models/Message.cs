using System;
using System.ComponentModel.DataAnnotations;

namespace Odin.Data.Core.Models
{
    public class Message : MobileTable
    {
        [Display(Name = "Message Date:")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", NullDisplayText = "NA")]
        public DateTime? MessageDate { get; set; }

        [Display(Name = "Message:")]
        public string MessageText { get; set; }

        public string HomeFindingPropertyId { get; set; }
        public HomeFindingProperty Property { get; set; }
        public void Delete()
        {
            Deleted = true;
        }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public bool IsRead { get; set; }
    }
}
