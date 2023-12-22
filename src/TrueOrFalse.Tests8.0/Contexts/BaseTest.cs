using Autofac;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Seedworks.Web.State;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
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
        
        Settings.Initialize(CreateConfiguration());
        SessionFactory.BuildTestConfiguration(); 
        // CleanEmailsFromPickupDirectory.Run();
        InitializeContainer();
        LifetimeScope = _container.BeginLifetimeScope();
        foreach (var registration in _container.ComponentRegistry.Registrations)
        {
            foreach (var service in registration.Services)
            {
                Console.WriteLine(service);
            }
        }

        var initilizer = Resolve<EntityCacheInitializer>();
        initilizer.Init(" (started in unit test) ");
        DateTimeX.ResetOffset();
    }

    [TearDown]
    public void RecycleContainer()
    {
        App.Environment = null;
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
        // JobScheduler.EmptyMethodToCallConstructor(); //Call here to have container with default solr cores registered (not suitable for unit testing) built first and overwritten afterwards 
        //var builder = new ContainerBuilder();
        //_container = builder.Build();
        var fakeWebHostEnvironment = A.Fake<IWebHostEnvironment>();
        App.Environment = fakeWebHostEnvironment;
        A.CallTo(() => fakeWebHostEnvironment.EnvironmentName).Returns("TestEnvironment");
        _container = AutofacWebInitializer.GetContainer(fakeWebHostEnvironment); 
    }


    private IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.Test.json"); // Pfad zur Konfigurationsdatei
        return configurationBuilder.Build();
    }
   

    // Jetzt können Sie Ihre Initialize-Methode mit dieser Konfiguration aufrufen
   

    public static T Resolve<T>() where T : notnull => _container.Resolve<T>();

    public static T R<T>() where T : notnull => _container.Resolve<T>();
}
