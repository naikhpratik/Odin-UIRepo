using Odin.Data.Core.Models;

namespace Odin.Data.Core.Dtos
{
    public class RentDto
    {
            public decimal? HousingBudget { get; set; }
        
             public int NumberOfBedrooms { get; set; }

            public int NumberOfBathrooms { get; set; }

            public int SquareFootage { get; set; }

            public OwnershipType OwnershipType { get; set; }

            public HousingType HousingType { get; set; }        
    }
}
