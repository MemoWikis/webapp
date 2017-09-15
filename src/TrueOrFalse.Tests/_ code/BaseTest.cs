using Autofac;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;

[TestFixture]
public class BaseTest
{
    private static IContainer _container;

    static BaseTest()
    {
        #if DEBUG
            NHibernateProfiler.Initialize();
        #endif
    }

    [SetUp]
    public void SetUp()
    {
        CleanEmailsFromPickupDirectory.Run();
        InitializeContainer();

        Resolve<SessionUser>().Login(new User());
        Resolve<SessionUser>().IsInstallationAdmin = true;

        DateTimeX.ResetOffset();

        EntityCache.Init(" (started in unit test) ");
    }

    public void RecycleContainer()
    {
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

    protected bool IsMysqlInMemoryEngine()  
    {
        return MySQL5FlexibleDialect.IsInMemoryEngine();
    }

    private static void BuildContainer()
    {
        JobScheduler.EmptyMethodToCallConstructor();//Call here to have container with default solr cores registered (not suitable for unit testing) built first and overwritten afterwards 

        var builder = new ContainerBuilder();
        builder.RegisterModule<AutofacCoreModule>();

        var solrUrl = Settings.SolrUrl;
        var cores = new SolrServers {
            new SolrServerElement {
                    Id = "question",
                    DocumentType = typeof (QuestionSolrMap).AssemblyQualifiedName,
                    Url = solrUrl + "tofQuestionTest"
                },
            new SolrServerElement {
                    Id = "set",
                    DocumentType = typeof (SetSolrMap).AssemblyQualifiedName,
                    Url = solrUrl + "tofSetTest"
                },
            new SolrServerElement {
                    Id = "category",
                    DocumentType = typeof (CategorySolrMap).AssemblyQualifiedName,
                    Url = solrUrl + "tofCategoryTest"
                },
            new SolrServerElement {
                    Id = "users",
                    DocumentType = typeof (UserSolrMap).AssemblyQualifiedName,
                    Url = solrUrl + "tofUserTest"
                }
        };

        builder.RegisterModule(new SolrNetModule(cores));
        _container = builder.Build();
        ServiceLocator.Init(_container);

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
