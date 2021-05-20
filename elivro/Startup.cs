using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(elivro.Startup))]
namespace elivro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
