//using Autofac;
//using Autofac.Integration.WebApi;
//using System;
//using RS.Core.Service;
//using System.Reflection;
//using System.Web.Http;

//namespace RS.Core.App_Start
//{
//    public class AutofacConfig
//    {
//        public static void Configure()
//        {
//            var builder = new ContainerBuilder();
//            builder.RegisterModule(new AutofacContext());
//            //Controlleri çekti
//            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
//            ///Tüm servisleri build eder.
//            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
//                .Where(x => x.Name.EndsWith("Service"))
//                .AsImplementedInterfaces()
//                .InstancePerRequest();
//            builder.RegisterType<EntityUnitofWork>().AsSelf().InstancePerRequest();
//            var container = builder.Build();
//            var resolver = new AutofacWebApiDependencyResolver(container);
//            GlobalConfiguration.Configuration.DependencyResolver = resolver;
//        }
//    }
//}