using Microsoft.AspNet.Identity;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Helpers;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class TransfereesRepository : ITransfereesRepository
    {
        private readonly ApplicationDbContext _context;

        public TransfereesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Transferee GetTransfereeByEmail(string email)
        {
            return _context.Transferees.SingleOrDefault(t => t.Email.Equals(email));
        }
        public Transferee GetTransfereeByOrderId(string id)
        {
            var eeId = _context.Orders.SingleOrDefault(o => o.Id == id).TransfereeId;
            return _context.Transferees.SingleOrDefault(t => t.Id.Equals(eeId));
        }
        public void Add(Transferee transferee)
        {
            var userManager = UserHelper.GetUserManager<Transferee>(_context);
            transferee.UserName = transferee.Email;
            var result = userManager.Create(transferee, TokenHelper.NewToken());
            userManager.AddToRole(transferee.Id, UserRoles.Transferee);
        }
    }
}
