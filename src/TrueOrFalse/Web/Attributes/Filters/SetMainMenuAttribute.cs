namespace System.Web.Mvc
{
    public class SetMainMenuAttribute : ActionFilterAttribute
    {
        private readonly MainMenuEntry _mainMenuEntry;

        public SetMainMenuAttribute(MainMenuEntry mainMenuEntry){
            _mainMenuEntry = mainMenuEntry;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.MainMenu.Current = _mainMenuEntry;

            base.OnActionExecuting(filterContext);
        }
    }
}