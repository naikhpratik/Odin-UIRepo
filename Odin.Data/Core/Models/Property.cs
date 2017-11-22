using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Property : MobileTable
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }

        public decimal? NumberOfBathrooms { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? SquareFootage { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? AvailabilityDate { get; set; }

        public string Description { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
