using System.Collections.Concurrent;
using System.Threading;
using Autofac;


public class ServiceLocator
{
    private static IContainer _container;

    private static readonly ConcurrentDictionary<int /*managed thread id*/, ILifetimeScope> _liftimeScopes = new();

    public static void AddScopeForCurrentThread(ILifetimeScope lifetimeScope)
    {
        if (!_liftimeScopes.TryAdd(Thread.CurrentThread.ManagedThreadId, lifetimeScope))
        {
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error("Could not add lifetime scope");
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
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error("Could not remove lifetime scope");
        }
    }
}