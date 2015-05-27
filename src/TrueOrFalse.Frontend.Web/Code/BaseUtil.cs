
public class BaseUtil
{
    protected SessionUser _sessionUser { get { return Resolve<SessionUser>(); } }
    protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }

    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }    
}