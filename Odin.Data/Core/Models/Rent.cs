using Microsoft.Azure.Mobile.Server;

namespace Odin.Data.Core.Models
{
    public class Rent : EntityData
    {
        public string Id { get; set; }

        public decimal? HousingBudget { get; set; }
        //TODO: The rest of the Housing Preferences
        
        public Order Order { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public int SquareFootage { get; set; }

        public OwnershipType OwnershipType { get; set; }

        public HousingType HousingType  { get; set; }
    }
}
