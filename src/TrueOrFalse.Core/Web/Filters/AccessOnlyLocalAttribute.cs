using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public class AccessOnlyLocalAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!HttpContext.Current.Request.IsLocal)
                throw new Exception("only local access is allowed");

            base.OnActionExecuting(filterContext);
        }
    }
}
