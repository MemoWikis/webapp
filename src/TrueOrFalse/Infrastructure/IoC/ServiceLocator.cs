using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

public class Sl
{
    public static T Resolve<T>(){return ServiceLocator.Resolve<T>();}
    public static T R<T>() { return ServiceLocator.Resolve<T>(); }
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