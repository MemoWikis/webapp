using System.Collections.Concurrent;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ServiceLocator
{
    private static IContainer _container;
    private static ThreadLocal<ILifetimeScope> _currentScope = new();

    private static readonly ConcurrentDictionary<int /*managed thread id*/, ILifetimeScope> _liftimeScopes = new();

    public static void AddScopeForCurrentThread(ILifetimeScope lifetimeScope)
    {
        if (!_liftimeScopes.TryAdd(Thread.CurrentThread.ManagedThreadId, lifetimeScope))
        {
            var httpContextAccessor = lifetimeScope.Resolve<IHttpContextAccessor>();
            var webHostEnvironment = lifetimeScope.Resolve<IWebHostEnvironment>();
            Logg.r.Error("Could not add lifetime scope");
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

    public static void RemoveScopeForCurrentThread()
    {

        if (!_liftimeScopes.TryRemove(Thread.CurrentThread.ManagedThreadId, out _))
        {
            var httpContextAccessor = _currentScope.Value.Resolve<HttpContextAccessor>(); 
            var webHostEnvironment = _currentScope.Value.Resolve<IWebHostEnvironment>();
            Logg.r.Error("Could not remove lifetime scope");
        }
    }
}