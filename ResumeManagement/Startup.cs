using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResumeManagement.Startup))]
namespace ResumeManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
