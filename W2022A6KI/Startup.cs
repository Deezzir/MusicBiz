using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(W2022A6KI.Startup))]

namespace W2022A6KI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
