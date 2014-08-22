using Autofac;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NUnit.Framework;
using SolrNet;
using SolrNet.Impl;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests
{
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
        }

        public void RecycleContainer()
        {
            R<ISession>().Flush();
            _container.Dispose();
            BuildContainer();
        }

        public static void InitializeContainer()
        {
            MySQL5FlexibleDialect.Engine = "MyISAM";
            BuildContainer();
            ServiceLocator.Init(_container);
            SessionFactory.BuildSchema();
        }

        private static void BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacCoreModule>();
            builder.RegisterModule<AutofacTestModule>();

            var solrUrl = WebConfigSettings.SolrUrl;
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
}
