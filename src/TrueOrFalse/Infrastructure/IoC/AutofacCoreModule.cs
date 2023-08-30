using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NHibernate;


using FluentNHibernate.Cfg; 
using Quartz;
using TrueOrFalse.Infrastructure.Persistence;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.Register(context => context.Resolve<SessionManager>().Session);
            builder.Register(c =>
            {
                var sessionFactory = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Database=memucho1;User ID=root;Password=Tassen12;"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Category>())
                    .BuildSessionFactory();
                return sessionFactory;
            }).As<ISessionFactory>().SingleInstance();

            builder.Register(context =>
                new SessionManager(context.Resolve<ISessionFactory>().OpenSession())
            ).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web"))
                .AssignableTo<IRegisterAsInstancePerLifetime>();

            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse");

            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IJob>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse)
                .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Repo"))
                .InstancePerLifetimeScope();

            builder.RegisterType<MemoryCache>()
                .As<IMemoryCache>()
                .SingleInstance();



            builder.RegisterType<MeiliGlobalSearch>().As<IGlobalSearch>();
        }
    }
}
