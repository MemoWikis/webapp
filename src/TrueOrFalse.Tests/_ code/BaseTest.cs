using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NUnit.Framework;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;

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

        private static void InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacCoreModule>();
            builder.RegisterModule<AutofacTestModule>();
            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }

}
