using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;

public class Sl
{
    public static T Resolve<T>(){return ServiceLocator.Resolve<T>();}
    public static T R<T>() { return ServiceLocator.Resolve<T>(); }

    public static ISession Session => R<ISession>();
    public static int CurrentUserId => R<SessionUser>().UserId;
}

public static class SlExt
{
    public static T R<T>(this object o)
    {
        return Sl.R<T>();
    }
}

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

    public static IContainer GetContainer()
    {
        return _container;
    }

    public static T R<T>(){
        return Resolve<T>();
    }
}