using System.Reflection;
using Autofac;
using NHibernate;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web"))
                   .AssignableTo<IRegisterAsInstancePerLifetime>();
            
            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse");
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse)
                .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Repo"));

            builder.RegisterInstance(SessionFactory.CreateSessionFactory());
            builder.Register(context => new SessionManager(context.Resolve<ISessionFactory>().OpenSession())).InstancePerLifetimeScope();
            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
        }
    }
}