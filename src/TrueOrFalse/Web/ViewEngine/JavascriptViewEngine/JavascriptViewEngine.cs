using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TrueOrFalse.Web.JavascriptView
{
    public class JavaScriptViewEngine : VirtualPathProviderViewEngine
    {
        public JavaScriptViewEngine()
            : this(null)
        {
        }

        public JavaScriptViewEngine(IViewPageActivator viewPageActivator)
            : base()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.js",
                "~/Areas/{2}/Views/Shared/{0}.js"
            };
            AreaMasterLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}/{0}.js",
                "~/Areas/{2}/Views/Shared/{0}.js"
            };
            AreaPartialViewLocationFormats = new []
            {
                "~/Areas/{2}/Views/{1}/{0}.js",
                "~/Areas/{2}/Views/Shared/{0}.js"
            };
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.js",
                "~/Views/Shared/{0}.js"
            };
            MasterLocationFormats = new[]
            {
                "~/Views/{1}/{0}.js",
                "~/Views/Shared/{0}.js"
            };
            PartialViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.js",
                "~/Views/Shared/{0}.js"
            };
            FileExtensions = new[]
            {
                "js"
            };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (viewName.EndsWith(".js"))
                viewName = viewName.Replace(".js", "");
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }


        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new JavaScriptView(partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new JavaScriptView(viewPath);
        }
    }
}
