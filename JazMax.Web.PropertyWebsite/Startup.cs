using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JazMax.Web.PropertyWebsite.Startup))]
namespace JazMax.Web.PropertyWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
