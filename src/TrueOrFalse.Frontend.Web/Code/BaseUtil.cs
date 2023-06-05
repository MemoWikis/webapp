
public class BaseUtil
{
    protected SessionUserLegacy SessionUserLegacy => Resolve<SessionUserLegacy>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();
    protected T R<T>() => ServiceLocator.Resolve<T>();
}