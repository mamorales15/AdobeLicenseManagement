using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdobeLicenseManagement.Startup))]
namespace AdobeLicenseManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
