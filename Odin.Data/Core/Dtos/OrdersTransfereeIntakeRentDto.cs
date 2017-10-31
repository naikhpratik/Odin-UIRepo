namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeRentDto
    {
        public string Id { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public byte? FurnishingTypeId { get; set; }
    }
}
