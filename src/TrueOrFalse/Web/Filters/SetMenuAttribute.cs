using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

namespace System.Web.Mvc
{
    public class SetMenuAttribute : ActionFilterAttribute
    {
        private readonly MenuEntry _menuEntry;

        public SetMenuAttribute(MenuEntry menuEntry){
            _menuEntry = menuEntry;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userSession = new SessionUiData();
            userSession.Menu.Current = _menuEntry;

            base.OnActionExecuting(filterContext);
        }
    }
}
