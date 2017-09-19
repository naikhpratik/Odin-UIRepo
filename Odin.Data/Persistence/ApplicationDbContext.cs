using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Core.Models;
using Odin.Data.Persistence.EntityConfigurations;

namespace Odin.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Transferee> Transferees { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new RentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}