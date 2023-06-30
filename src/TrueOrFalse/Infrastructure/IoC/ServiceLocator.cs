using System.Collections.Concurrent;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TrueOrFalse.Search;

public class Sl
{
    public static QuestionChangeRepo QuestionChangeRepo => R<QuestionChangeRepo>();

    public static QuestionValuationRepo QuestionValuationRepo => R<QuestionValuationRepo>();
    public static UserRepo UserRepo => R<UserRepo>();

    public static T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    public static T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
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

    private static readonly ConcurrentDictionary<int /*managed thread id*/, ILifetimeScope> _liftimeScopes = new();

    public static void AddScopeForCurrentThread(ILifetimeScope lifetimeScope)
    {
        if (!_liftimeScopes.TryAdd(Thread.CurrentThread.ManagedThreadId, lifetimeScope))
        {
            Logg.r().Error("Could not add lifetime scope");
        }
    }

    public static IContainer GetContainer()
    {
        return _container;
    }

    public static void Init(IContainer container)
    {
        _container = container;
    }

    public static T R<T>()
    {
        return Resolve<T>();
    }

    public static void RemoveScopeForCurrentThread()
    {
        if (!_liftimeScopes.TryRemove(Thread.CurrentThread.ManagedThreadId, out _))
        {
            Logg.r().Error("Could not remove lifetime scope");
        }
    }

    public static T Resolve<T>()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        if (_liftimeScopes.ContainsKey(currentThreadId))
        {
            return _liftimeScopes[currentThreadId].Resolve<T>();
        }

        if (HttpContext.Current == null)
        {
            return _container.Resolve<T>();
        }

        return ((AutofacDependencyResolver)DependencyResolver.Current).RequestLifetimeScope.Resolve<T>();
    }
}