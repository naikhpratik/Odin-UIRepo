namespace Odin.Data.Core.Models
{
    public class Lease : MobileTable
    {
        public Order Order { get; set; }
       
        public DepositType DepositType { get; set; }

        public byte? DepositTypeId { get; set; }

        public int? LeaseTerm { get; set; }

        public BrokerFeeType BrokerFeeType { get; set; }

        public byte? BrokerFeeTypeId { get; set; }

        public int? LengthOfAssignment { get; set; }

    }
}
