using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UanlSISM.Startup))]
namespace UanlSISM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
