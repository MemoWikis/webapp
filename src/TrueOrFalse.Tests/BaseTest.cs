using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Core;
using IContainer = System.ComponentModel.IContainer;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class BaseTest
    {
        private IContainer _container;

        [Test]
        public void SetUp()
        {
            InitializeContainer();
        }

        private void InitializeContainer()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<SampleEntityService>();
            //builder.RegisterType<SampleEntityRepository>();
            //builder.RegisterInstance(SessionFactory.CreateSessionFactory().OpenSession());
            //_container = builder.Build();
        }
    }
}
