using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Travosaur.Startup))]
namespace Travosaur
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
