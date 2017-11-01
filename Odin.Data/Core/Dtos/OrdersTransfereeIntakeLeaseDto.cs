namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeLeaseDto
    {
        public string Id { get; set; }

        public byte? DepositTypeId { get; set; }

        public int? LeaseTerm { get; set; }

        public byte? BrokerFeeTypeId { get; set; }

        public int? LengthOfAssignment { get; set; }
    }
}
