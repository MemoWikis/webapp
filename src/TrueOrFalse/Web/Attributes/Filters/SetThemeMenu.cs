namespace System.Web.Mvc
{
    public class SetThemeMenu : ActionFilterAttribute
    {
        private bool _hasBelongingCategory;

        public SetThemeMenu(bool hasBelongingCategory = false)
        {
            _hasBelongingCategory = hasBelongingCategory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.ThemeMenu.IsActive = true;

            if (_hasBelongingCategory)
            {
                var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;
                var currentCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));
                userSession.ThemeMenu.ActiveCategory = currentCategory;
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.ThemeMenu.IsActive = false;
            userSession.ThemeMenu.ActiveCategory = null;

            base.OnResultExecuted(filterContext);
        }
    }
}