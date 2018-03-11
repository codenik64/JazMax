using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JazMax.Web.Startup))]
namespace JazMax.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
