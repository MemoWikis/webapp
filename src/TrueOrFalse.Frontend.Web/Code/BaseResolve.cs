public class BaseResolve
{
    protected T Resolve<T>() => ServiceLocator.Resolve<T>();
    protected T R<T>() => Resolve<T>();
}
