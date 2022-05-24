using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AvisFormation.WebUI.Startup))]
namespace AvisFormation.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
