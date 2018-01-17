namespace Odin.Data.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Lease : MobileTable
    {

        public string PropertyId { get; set; }
        public Property Property { get; set; }
        public string pmEmail { get; set; }
        public Transferee transferee { get; set; }
        public string Tenant { get; set; }
        public string LandLord { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentIncrease { get; set; }
        public decimal SecurityDeposit { get; set; }
        public string SecurityDepositTerms { get; set; }
        public string LeaseEndNoticeTerms { get; set; }
        public string RenewalTerms { get; set; }
        public string DiplomatTerms { get; set; }
        public string EarlyTerminationTerms { get; set; }
        public string NotableClauses { get; set; }
        public string PaymentInformation { get; set; }
        public decimal InitialRentAmount { get; set; }
        public DateTime? InitialRentDueDate { get; set; }
        public string InitialRentPaideTo { get; set; }
        public DateTime? SecurityDepositDueDate { get; set; }
        public string SecurityDepositPaideTo { get; set; }
        public decimal FirstOnGoingRentAmount { get; set; }
        public DateTime? FirstOnGoingRentDueDate { get; set; }
        public string FirstOnGoingRentPaideTo { get; set; }
    }   
}