using Odin.Data.Core.Models;
using System.Data.Entity;

namespace Odin.Data.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<Rent> Rents { get; set; }
        DbSet<Transferee> Transferees { get; set; }
        DbSet<Consultant> Consultants { get; set; }
        DbSet<Manager> Managers { get; set; }
        DbSet<Child> Children { get; set; }
        DbSet<Pet> Pets { get; set; }
    }
}
