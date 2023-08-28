using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NHibernate;
using NHibernate.Cfg;
using Quartz;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.Register(c =>
            {
                var sessionFactory = new Configuration()
                    .Configure("hibernate.cfg.xml") 
                    .BuildSessionFactory();
                return sessionFactory;
            }).As<ISessionFactory>().SingleInstance();

            builder.Register(context =>
                new SessionManager(context.Resolve<ISessionFactory>().OpenSession())
            ).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web"))
                .AssignableTo<IRegisterAsInstancePerLifetime>();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse");

            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IJob>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse)
                .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Repo"));

            builder.RegisterType<MemoryCache>()
                .As<IMemoryCache>()
                .SingleInstance();

     

            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
            builder.RegisterType<MeiliGlobalSearch>().As<IGlobalSearch>();
        }
    }
}
