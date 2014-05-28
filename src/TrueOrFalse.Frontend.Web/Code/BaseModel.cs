using TrueOrFalse.Web.Context;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class BaseModel : BaseResolve
    {
        public MenuLeftModel MenuLeftModel = new MenuLeftModel();

        protected SessionUser _sessionUser { get { return Resolve<SessionUser>(); } }
        protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }

        public bool IsLoggedIn{ get { return _sessionUser.IsLoggedIn; } }

        public int UserId
        {
            get
            {
                if (_sessionUser.IsLoggedIn)
                    return _sessionUser.User.Id;

                return -1;
            }
        }
    }
}