using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5HomeWork.Startup))]
namespace MVC5HomeWork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
