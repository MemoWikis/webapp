using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Web.JavascriptView;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Questions", "Questions", new { controller = "Questions", action = "Questions" });
            routes.MapRoute("Question_Create", "Questions/Create/{action}", new { controller = "CreateQuestion", action = "Create" });
            routes.MapRoute("Categories", "Categories", new { controller = "Categories", action = "Categories" });
            routes.MapRoute("Categories_Create", "Categories/Create/{action}", new { controller = "EditCategory", action = "Create" });
            routes.MapRoute("Knowledge", "Knowledge/{action}", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("News", "News/{action}", new { controller = "News", action = "News" });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });          
            routes.MapRoute("Export", "Api/Export/{action}", new { controller = "Export", action="Questions" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = "" });
        }

        protected void Application_Start()
        {
            InitializeAutofac();

            AreaRegistration.RegisterAllAreas();
            ViewEngines.Engines.Add(new JavaScriptViewEngine());

            RegisterRoutes(RouteTable.Routes);
            
            GlobalFilters.Filters.Add(new GlobalAuthorizationAttribute());
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModule<AutofacCoreModule>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}