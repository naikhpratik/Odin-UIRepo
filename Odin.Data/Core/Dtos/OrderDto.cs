using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class OrderDto
    {
        [Required]
        public string TrackingId { get; set; }
        public string RelocationType { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZip { get; set; }
        public string DestinationCountry { get; set; }
        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginCountry { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }
        public bool TransfereeInviteEnabled { get; set; }

        [Required]
        public TransfereeDto Transferee { get; set; }

        [Required]
        public ProgramManagerDto ProgramManager { get; set; }

        [Required]
        public ConsultantDto Consultant { get; set; }
    }
}
