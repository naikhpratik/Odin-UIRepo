using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyDwellworks.Startup))]
namespace MyDwellworks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
