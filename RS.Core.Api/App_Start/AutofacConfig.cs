using Autofac;
using Autofac.Integration.WebApi;
using RS.Core.Service;
using RS.Core.Service.App_Start;
using System;
using System.Reflection;
using System.Web.Http;

namespace RS.Core.App_Start
{
    public class AutofacConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            //Context
            builder.RegisterModule(new AutofacContext());

            //Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Services
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            //UnitofWork
            builder.RegisterType<EntityUnitofWork<Guid>>().AsSelf().InstancePerRequest();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            return container;
        }
    }
}