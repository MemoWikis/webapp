using System.Collections.Concurrent;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TrueOrFalse.Search;

public class Sl
{
    public static bool IsUnitTest = false;

    public static AnswerRepo AnswerRepo => R<AnswerRepo>();
    public static CategoryChangeRepo CategoryChangeRepo => R<CategoryChangeRepo>();

    public static CategoryRepository CategoryRepo => R<CategoryRepository>();
    public static CategoryValuationRepo CategoryValuationRepo => R<CategoryValuationRepo>();
    public static CategoryViewRepo CategoryViewRepo => R<CategoryViewRepo>();

    public static ImageMetaDataRepo ImageMetaDataRepo => R<ImageMetaDataRepo>();

    public static JobQueueRepo JobQueueRepo => R<JobQueueRepo>();
    public static MeiliSearchQuestions MeilieMeiliSearchQuestions => R<MeiliSearchQuestions>();
    public static MeiliSearchUsers MeiliSearchUsers => R<MeiliSearchUsers>();
    public static QuestionChangeRepo QuestionChangeRepo => R<QuestionChangeRepo>();

    public static QuestionRepo QuestionRepo => R<QuestionRepo>();
    public static QuestionValuationRepo QuestionValuationRepo => R<QuestionValuationRepo>();
    public static SessionUiData SessionUiData => R<SessionUiData>();
    public static SetRepo SetRepo => R<SetRepo>();
    public static UserActivityRepo UserActivityRepo => R<UserActivityRepo>();

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