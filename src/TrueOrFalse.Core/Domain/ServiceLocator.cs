using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace TrueOrFalse.Core
{
    public class ServiceLocator
    {
        private static IContainer _container;

        public static void Init(IContainer container)
        {
            _container = container;
        }

        public static T Resolve<T>()
        {
            if (HttpContext.Current == null)
                return _container.Resolve<T>();

            return ((AutofacDependencyResolver) DependencyResolver.Current).RequestLifetimeScope.Resolve<T>();
        }
    }
}
