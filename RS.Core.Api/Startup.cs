using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using RS.Core.Service.AutoMapper;

[assembly: OwinStartup(typeof(RS.Core.Startup))]

namespace RS.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AutoMapperConfig.Configure();
        }
    }
}
