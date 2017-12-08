using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Odin.Data.Core;
using Odin.Data.Persistence;

namespace Odin.PropBotWebJob
{
    public class MyBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationDbContext>().To<ApplicationDbContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
