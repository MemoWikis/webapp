namespace System.Web.Mvc
{
    public class SetThemeMenu : ActionFilterAttribute
    {
        private readonly bool _isVisible;
        private readonly Category _currentCategory;

        public SetThemeMenu(bool hasBelongingCategory){
            _isVisible = true;

            if (hasBelongingCategory)
            {
                var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;
                _currentCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.ThemeMenu.IsVisible = _isVisible;
            userSession.ThemeMenu.ActualCategory = _currentCategory;

            base.OnActionExecuting(filterContext);
        }
    }
}