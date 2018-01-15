using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Odin.Data.Persistence
{
    public class LeasesRepository : ILeasesRepository
    {
        private readonly ApplicationDbContext _context;

        public LeasesRepository(ApplicationDbContext context)
        {
            _context = context;
        }        

        public void Add(Lease lease)
        {
            _context.Leases.Add(lease);
        }
        public Lease GetLeaseByPropertyId(string id)
        {
            var lease = _context.Leases.Where(a => a.PropertyId == id && a.Deleted == false).SingleOrDefault<Lease>();                 
            if (lease == null)
            {
                var property = _context.Properties.Where(p => p.Id == id).First();
                lease = new Lease();
                lease.Property = property;
            }
            return lease;
        }
        public Lease GetLeaseById(string id)
        {
            return _context.Leases.Where(a => a.Id.Equals(id)).SingleOrDefault<Lease>(); 
        }
        public void Remove(Lease lease)
        {
            _context.Leases.Remove(lease);
        }
    }
}
