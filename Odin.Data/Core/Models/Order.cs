using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        
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
        public string FamilyDetails { get; set; }
        
        public int? RentId { get; set; }
        public Rent Rent { get; set; }
        
        public string TransfereeId { get; set; }
        public ApplicationUser Transferee { get; set; }

        public string ProgramManagerId { get; set; }
        public ApplicationUser ProgramManager { get; set; }

        public string ConsultantId { get; set; }
        public ApplicationUser Consultant { get; set; }
        
    }
}
