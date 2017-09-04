using Microsoft.Owin;
using Owin;
using RS.Core.App_Start;
using RS.Core.Service.AutoMapper;

[assembly: OwinStartup(typeof(RS.Core.Startup))]

namespace RS.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Autofac
            app.UseAutofacMiddleware(AutofacConfig.Configure());

            //Identity
            ConfigureAuth(app);

            //AutoMapper
            AutoMapperConfig.Configure();
        }
    }
}
