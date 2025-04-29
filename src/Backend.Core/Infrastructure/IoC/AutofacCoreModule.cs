using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NHibernate;
using Quartz;
using System.Reflection;
using System.Text;

public class AutofacCoreModule : Autofac.Module
{
    private readonly bool _externallyProvidedHttpContextAccessor;

    public AutofacCoreModule()
    {
    }

    public AutofacCoreModule(bool externallyProvidedHttpContextAccessor)
    {
        _externallyProvidedHttpContextAccessor = externallyProvidedHttpContextAccessor;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_externallyProvidedHttpContextAccessor == false)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }

        builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();



        builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();

        builder.Register(c => Options.Create(new MemoryCacheOptions()))
            .As<IOptions<MemoryCacheOptions>>()
            .SingleInstance();

        // Register MemoryCache
        builder.RegisterType<MemoryCache>()
            .As<IMemoryCache>();
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
        builder.RegisterAssemblyTypes(Assembly.Load("MemoWikis.Backend.Api"))
            .AssignableTo<IRegisterAsInstancePerLifetime>();

        var assemblyBackendCore = Assembly.Load("MemoWikis.Backend.Core");

        builder.RegisterAssemblyTypes(assemblyBackendCore).AssignableTo<IRegisterAsInstancePerLifetime>();
        builder.RegisterType<EntityCacheInitializer>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(assemblyBackendCore).AssignableTo<IJob>();
        builder.RegisterAssemblyTypes(assemblyBackendCore)
            .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Repo"))
            .InstancePerLifetimeScope();

        builder.RegisterType<MemoryCache>()
            .As<IMemoryCache>()
            .SingleInstance();

        builder.RegisterType<MeiliGlobalSearch>().As<IGlobalSearch>();
    }
}