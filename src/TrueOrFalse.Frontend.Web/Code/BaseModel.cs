using TrueOrFalse.Web.Context;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class BaseModel
    {
        public MenuLeftModel MenuLeftModel = new MenuLeftModel();

        protected SessionUser _sessionUser { get { return Resolve<SessionUser>(); } }
        protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }

        protected T Resolve<T>()
        {
            return ServiceLocator.Resolve<T>();
        }
    }
}