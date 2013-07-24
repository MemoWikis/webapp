using Autofac;
using AutofacContrib.SolrNet;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NUnit.Framework;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;

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
            BuildContainer();
        }

        private static void InitializeContainer()
        {
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
            var solrOverwritten = ReadOverwrittenConfig.SolrUrl();
            if(solrOverwritten.HasValue)
                solrUrl = solrOverwritten.Value;

            builder.RegisterModule(new SolrNetModule(solrUrl + "tofQuestion"));
            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
