using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDwellworks.Data.Core;
using MyDwellworks.Data.Core.Repositories;

namespace MyDwellworks.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUsersRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UsersRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
