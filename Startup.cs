using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JTicket.Startup))]
namespace JTicket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
