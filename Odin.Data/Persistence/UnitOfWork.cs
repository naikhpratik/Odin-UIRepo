﻿using Odin.Data.Core;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUsersRepository Users { get; private set; }
        public IOrdersRepository Orders { get; private set; }
        public ITransfereesRepository Transferees { get; private set; }
        public IConsultantsRepository Consultants { get; private set; }
        public IManagersRepository Managers { get; private set; }
        public IServicesRepository Services { get; private set; }
        public IServiceTypesRepository ServiceTypes { get; private set; }
        public IChildrenRepository Children { get; private set; }
        public IPetsRepository Pets { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UsersRepository(context);
            Orders = new OrdersRepository(context);
            Transferees = new TransfereesRepository(context);
            Consultants = new ConsultantsRepository(context);
            Managers = new ManagersRepository(context);
            Services = new ServicesRepository(context);
            ServiceTypes = new ServiceTypesRepository(context);
            Children = new ChildrenRepository(context);
            Pets = new PetsRepository(context);
        }
        
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
