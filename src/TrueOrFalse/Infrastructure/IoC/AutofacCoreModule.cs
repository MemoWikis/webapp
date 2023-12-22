using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NHibernate;
using Quartz;
using TrueOrFalse.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            

            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
            try
            {
#if DEBUG
                var interceptor = new SqlDebugOutputInterceptor();
                var sessionBuilder = SessionFactory.CreateSessionFactory().WithOptions().Interceptor(interceptor);
#else
                var sessionBuilder = SessionFactory.CreateSessionFactory().WithOptions().NoInterceptor();
#endif 

                builder.RegisterInstance(sessionBuilder);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    if (innerException is ReflectionTypeLoadException)
                        break;

                    innerException = innerException.InnerException;
                }

                if (innerException == null)
                    throw;

                foreach (Exception exSub in ((ReflectionTypeLoadException)innerException).LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }

                throw new Exception(sb.ToString());
            }

            builder.Register(context => new SessionManager(context.Resolve<ISessionBuilder>().OpenSession())).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web"))
                .AssignableTo<IRegisterAsInstancePerLifetime>();

            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse");

            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterType<EntityCacheInitializer>().AsSelf().InstancePerLifetimeScope();
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
