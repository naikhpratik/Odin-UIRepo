using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MyDwellworks.App_Start;
using Ninject.Web.WebApi;
using Odin.Data.Persistence;
using Owin;

namespace Odin.IntegrationTests
{
    public class OwinTestConf
    {
        public void Configuration(IAppBuilder app)
        {
            NinjectWebCommon.Start();
            var config = new HttpConfiguration();
            var kernel = NinjectWebCommon.bootstrapper.Kernel;

            var resolver = new NinjectDependencyResolver(kernel);
            config.DependencyResolver = resolver;
            WebApiConfig.Register(config);
            
        }
    }
}
