using TrueOrFalse;

public class BaseResolve
{
    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
}
