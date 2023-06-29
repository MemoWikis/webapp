using Autofac;
using NHibernate;
using NUnit.Framework;
using Seedworks.Web.State;
using TrueOrFalse;
using TrueOrFalse.Utilities.ScheduledJobs;

[TestFixture]
public class BaseTest
{
    private static IContainer _container;
    protected ILifetimeScope LifetimeScope;
    static BaseTest()
    {
        #if DEBUG
//            NHibernateProfiler.Initialize();
        #endif

    }

    [SetUp]
    public void SetUp()
    {
        CleanEmailsFromPickupDirectory.Run();
        InitializeContainer();
        LifetimeScope = _container.BeginLifetimeScope(); 

        Resolve<EntityCacheInitializer>().Init(" (started in unit test) ");
        DateTimeX.ResetOffset();
    }

    [TearDown]
    public void RecycleContainer()
    {
        EntityCache.Clear();
        Resolve<SessionData>().Clear();
        R<ISession>().Flush();
        _container.Dispose();
        BuildContainer();
    }

    public static void InitializeContainer()
    {
        MySQL5FlexibleDialect.Engine = "MEMORY";
        BuildContainer();
        ServiceLocator.Init(_container);
        SessionFactory.TruncateAllTables();
        
    }

    private static void BuildContainer()
    {
        JobScheduler.EmptyMethodToCallConstructor(); //Call here to have container with default solr cores registered (not suitable for unit testing) built first and overwritten afterwards 
        var builder = new ContainerBuilder();
        _container = builder.Build();
    }

    public static T Resolve<T>()
    {
        return _container.Resolve<T>();
    }

    public static T R<T>()
    {
        return _container.Resolve<T>();
    }
}
