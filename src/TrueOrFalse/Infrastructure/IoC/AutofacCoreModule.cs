using System.Reflection;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NHibernate;
using Quartz;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

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

            builder.Register(c =>
            {
                try
                {
                    var accessor = c.Resolve<IHttpContextAccessor>();
                    var environment = c.Resolve<IWebHostEnvironment>();

#if DEBUG
                    var interceptor = new SqlDebugOutputInterceptor();
                    if (accessor.HttpContext == null)
                    {
                        Logg.r().Error(new NullReferenceException(), "HttpContext is null - AutofacCoreModule/Load");
                    }

                    return SessionFactory.CreateSessionFactory(accessor.HttpContext, environment).WithOptions().Interceptor(interceptor);
#else
        return SessionFactory.CreateSessionFactory().WithOptions().NoInterceptor();
#endif
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
                        if (exSub is FileNotFoundException exFileNotFound)
                        {
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

            }).As<ISessionFactory>().SingleInstance();


            builder.Register(context => new SessionManager(context.Resolve<ISessionBuilder>().OpenSession())).InstancePerLifetimeScope();
            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
            builder.RegisterType<MeiliGlobalSearch>().As<IGlobalSearch>();
        }
    }
}
