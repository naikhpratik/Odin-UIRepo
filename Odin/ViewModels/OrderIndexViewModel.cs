using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Odin.Data.Core.Models;

namespace Odin.ViewModels
{
    public class OrderIndexViewModel
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
        
        public TransfereeViewModel Transferee { get; set; }

        public ConsultantViewModel Consultant { get; set; }
    }
}