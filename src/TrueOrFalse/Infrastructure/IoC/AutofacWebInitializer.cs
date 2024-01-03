using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.Infrastructure
{
    public static class AutofacWebInitializer
    {
        private static IContainer? _container;

        public static IContainer GetTestContainer(
            IWebHostEnvironment fakeEnvironment, 
            IHttpContextAccessor httpContextAccessor)
        {
            if (_container == null)
            {
                _container = InitializeTest(fakeEnvironment, httpContextAccessor);
            }
            return _container;
        }

        public static void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }

        private static IContainer InitializeTest(
            IWebHostEnvironment fakeEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(fakeEnvironment)
                .As<IWebHostEnvironment>()
                .SingleInstance();
            builder.RegisterInstance(httpContextAccessor)
                .As<IHttpContextAccessor>()
                .SingleInstance();
            builder.RegisterModule(new AutofacCoreModule(true));
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

            builder.RegisterModule(new AutofacCoreModule());
            return builder.Build();
        }
    }
}