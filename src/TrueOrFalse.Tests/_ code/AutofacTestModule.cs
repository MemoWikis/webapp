using System.Reflection;
using Autofac;
using TrueOrFalse;
using Module = Autofac.Module;

public class AutofacTestModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.Tests")).AssignableTo<IRegisterAsInstancePerLifetime>();

    }
}