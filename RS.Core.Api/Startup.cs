using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using RS.Core.App_Start;
using RS.Core.Service.AutoMapper;
using System;

[assembly: OwinStartup(typeof(RS.Core.Startup))]

namespace RS.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Cors
            app.UseCors(CorsOptions.AllowAll);

            //Autofac
            app.UseAutofacMiddleware(AutofacConfig.Configure());

            //Identity
            ConfigureAuth(app);

            //AutoMapper
            AutoMapperConfig<Guid>.Configure();
        }
    }
}
