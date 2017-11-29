using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin;
using MyDwellworks.App_Start;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Odin;
using Odin.App_Start;
using Odin.Helpers;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Odin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ConfigureAuth(app);
            app.UseNinjectMiddleware(NinjectWebCommon.CreateKernel);
            app.UseNinjectWebApi(configuration);
            AzureStorageStartup.StartLocalAzureStorageEmulator();
        }
    }
}
