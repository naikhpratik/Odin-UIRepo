using Microsoft.Owin;
using Odin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Odin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
