using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IManagersRepository
    {
        Manager GetManagerBySeContactUid(int seContactUid);
        void Add(Manager manager, string userRole);
    }
}
