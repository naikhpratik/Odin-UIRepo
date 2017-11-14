using Odin.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class HomeFindingPropertyDto
    {
        public String OrderId { get; set; }

        public String Street1 { get; set; }
        public String Street2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String CountryCode { get; set; }

        public NumberOfBathroomsType NumberOfBathrooms { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? SquareFootage { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? AvailabilityDate { get; set; }

        public String Description { get; set; }
    }
}
