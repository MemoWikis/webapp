using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.Infrastructure
{
    public static class AutofacWebInitializer
    {
        private static IContainer? _container;

        public static IContainer GetContainer(IWebHostEnvironment? fakEnvironment = null)
        {
            if (_container == null)
            {
                if (fakEnvironment == null)
                    _container = Initialize();
                else
                    _container = InitializeTest(fakEnvironment);

            }
            return _container;
        }

        private static IContainer InitializeTest(IWebHostEnvironment fakEnvironment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(fakEnvironment)
                .As<IWebHostEnvironment>()
                .SingleInstance();
            builder.RegisterModule<AutofacCoreModule>();
            return builder.Build();
        }

        private static IContainer Initialize(bool registerForAspNet = false, Assembly assembly = null)
        {
            var builder = new ContainerBuilder();

            if (registerForAspNet)
            {
                builder.RegisterAssemblyModules(assembly);
            }

            builder.RegisterModule<AutofacCoreModule>();
            return builder.Build();
        }

        public static IContainer Run(
            bool registerForAspNet = false,
            Assembly assembly = null)
        {
            var builder = new ContainerBuilder();

            if (registerForAspNet && assembly != null)
            {
                // Register controllers
                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => typeof(ControllerBase).IsAssignableFrom(t)) // Ensure it's a Controller
                    .InstancePerLifetimeScope();
            }

            builder.RegisterModule<AutofacCoreModule>();
            return builder.Build();
        }
    }
}