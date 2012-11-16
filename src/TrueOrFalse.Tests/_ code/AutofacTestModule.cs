using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace TrueOrFalse.Tests
{
    public class AutofacTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.Tests")).AssignableTo<IRegisterAsInstancePerLifetime>();

        }
    }
}
