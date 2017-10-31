namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeRentDto
    {
        public string Id { get; set; }

        public int? NumberOfBedrooms { get; set; }

        public byte? NumberOfBathroomsTypeId { get; set; }

        public decimal? HousingBudget { get; set; }

        public int? SquareFootage { get; set; }

        public int? MaxCommute { get; set; }

        public string Comments { get; set; }

        public int? NumberOfCarsOwned { get; set; }

        public bool? IsFurnished { get; set; }

        public bool? HasParking { get; set; }

        public bool? HasLaundry { get; set; }

        public bool? HasAC { get; set; }

        public bool? HasExerciseRoom { get; set; }

        public byte? HousingTypeId { get; set; }

        public byte? AreaTypeId { get; set; }

        public byte? TransportationTypeId { get; set; }
    }
}
