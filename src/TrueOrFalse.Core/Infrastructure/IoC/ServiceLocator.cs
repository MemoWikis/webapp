using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace TrueOrFalse.Core
{
    public class ServiceLocator
    {
        public static T Resolve<T>()
        {
            return ((AutofacDependencyResolver) DependencyResolver.Current).RequestLifetimeScope.Resolve<T>();
        }
    }
}
