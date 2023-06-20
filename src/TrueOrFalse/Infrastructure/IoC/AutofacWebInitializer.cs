using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;

namespace TrueOrFalse.Infrastructure
{
    public static class AutofacWebInitializer
    {
        public static IContainer Run(
            bool registerForAspNet = false,
            Assembly assembly = null)
        {
            var builder = new ContainerBuilder();

            if (registerForAspNet)
            {
                builder.RegisterControllers(assembly);
                builder.RegisterModelBinders(assembly);
            }

            builder.RegisterModule<AutofacCoreModule>();
            return builder.Build();
        }
    }
}