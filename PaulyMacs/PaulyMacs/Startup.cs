using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaulyMacs.Startup))]
namespace PaulyMacs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
