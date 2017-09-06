using Autofac;
using RS.Core.Data;

namespace RS.Core.Service.App_Start
{
    public class AutofacContext : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(RSCoreDBContext)).AsSelf().InstancePerLifetimeScope();
        }

    }
}
