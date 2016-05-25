using Owin;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}