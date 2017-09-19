using System.Collections.Generic;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<ApplicationUser> GetUsersWithRole(string roleName);
        ApplicationUser GetUserByEmail(string email);
        ApplicationUser GetUserByUserName(string userName);
    }
}
