using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using System.Collections.Generic;
using System.Threading;

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
    private static readonly Dictionary<int /*managed thread id*/, ILifetimeScope> _liftimeScopes = new Dictionary<int, ILifetimeScope>();

    public static void Init(IContainer container)
    {
        _container = container;
    }

    public static void AddScopeForCurrentThread(ILifetimeScope lifetimeScope)
    {
        _liftimeScopes.Add(Thread.CurrentThread.ManagedThreadId, lifetimeScope);
        Logg.r().Information("AddScopeForCurrentThread {ManagedThreadId} {LifetimeScopeTag}", Thread.CurrentThread.ManagedThreadId, lifetimeScope.Tag);
    }

    public static void RemoveScopeForCurrentThread()
    {
        var tag = _liftimeScopes[Thread.CurrentThread.ManagedThreadId].Tag;
        _liftimeScopes.Remove(Thread.CurrentThread.ManagedThreadId);
        var keys = string.Join(",", _liftimeScopes.Keys);
        Logg.r().Information("RemoveScopeForCurrentThread {ManagedThreadId} {LifetimeScopeTag}, Active threads: " + keys, Thread.CurrentThread.ManagedThreadId, tag);
    }

    public static T Resolve<T>()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        if (_liftimeScopes.ContainsKey(currentThreadId))
        {
            var scope = _liftimeScopes[currentThreadId];

            Logg.r().Information("ResolveFromScope {ManagedThreadId} {LifetimeScopeTag}", Thread.CurrentThread.ManagedThreadId, scope.Tag);
            
            //2: what happens on exception?

            return scope.Resolve<T>();
        }

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