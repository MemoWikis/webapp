namespace TrueOrFalse.Frontend.Web.Models
{
    public class BaseResolve
    {
        protected T Resolve<T>()
        {
            return ServiceLocator.Resolve<T>();
        }
    }
}