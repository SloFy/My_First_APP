using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(My_First_APP.Startup))]
namespace My_First_APP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
