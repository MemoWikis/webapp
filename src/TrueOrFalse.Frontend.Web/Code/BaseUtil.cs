
public class BaseUtil
{
    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();
    protected T R<T>() => ServiceLocator.Resolve<T>();
}