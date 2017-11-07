using Odin.Data.Core.Models;
using System.Data.Entity;

namespace Odin.Data.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<HomeFinding> HomeFindings { get; set; }
        DbSet<Transferee> Transferees { get; set; }
        DbSet<Consultant> Consultants { get; set; }
        DbSet<Manager> Managers { get; set; }
        DbSet<Child> Children { get; set; }
        DbSet<Pet> Pets { get; set; }
        DbSet<NumberOfBathroomsType> NumberOfBathrooms { get; set; }
        DbSet<HousingType> HousingTypes { get; set; }
        DbSet<AreaType> AreaTypes { get; set; }
        DbSet<TransportationType> TransportationTypes { get; set; }
        DbSet<DepositType> DepositTypes { get; set; }
        DbSet<BrokerFeeType> BrokerFeeTypes { get; set; }
    }
}
