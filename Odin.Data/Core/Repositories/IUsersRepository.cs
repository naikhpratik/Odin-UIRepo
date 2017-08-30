using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDwellworks.Data.Core.Models;

namespace MyDwellworks.Data.Core.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<ApplicationUser> GetUsersWithRole(string roleName);
    }
}
