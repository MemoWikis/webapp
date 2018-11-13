namespace System.Web.Mvc
{
    public class SetUserMenuAttribute : ActionFilterAttribute
    {
        private readonly UserMenuEntry _userMenuEntry;

        public SetUserMenuAttribute(UserMenuEntry userMenuEntry){
            _userMenuEntry = userMenuEntry;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.UserMenu.Current = _userMenuEntry;

            base.OnActionExecuting(filterContext);
        }
    }
}