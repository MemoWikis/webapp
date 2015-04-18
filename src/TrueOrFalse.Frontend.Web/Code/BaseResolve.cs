public class BaseResolve
{
    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return Resolve<T>();
    }
}
