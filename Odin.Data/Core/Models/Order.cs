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
        public Order()
        {
            Consultants = new Collection<ConsultantAssignment>();
        }

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

        public int TransfereeId { get; set; }
        public Transferee Transferee { get; set; }

        public ApplicationUser ProgramManager { get; set; }
        public string ProgramManagerId { get; set; }

        public ICollection<ConsultantAssignment> Consultants { get; set; }
    }
}
