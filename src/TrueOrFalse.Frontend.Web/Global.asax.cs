using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;
using RegistrationExtensions = Autofac.Integration.Mvc.RegistrationExtensions;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication, IContainerProviderAccessor 
    {
        static IContainerProvider _containerProvider;

        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Questions", "Questions", new { controller = "Questions", action = "Questions" });
            routes.MapRoute("Question_Create", "Questions/Create/{action}", new { controller = "CreateQuestion", action = "Create" });
            routes.MapRoute("Categories", "Categories", new { controller = "Categories", action = "" });
            routes.MapRoute("Categories_Create", "Categories/Create/{action}", new { controller = "EditCategory", action = "Create" });
            routes.MapRoute("Knowledge", "Knowledge/{action}", new { controller = "Knowledge", action = "Knowledge" });
            routes.MapRoute("News", "News/{action}", new { controller = "News", action = "News" });
            routes.MapRoute("Various", "{action}", new { controller = "VariousPublic" });
            routes.MapRoute("Default","{controller}/{action}/{id}", new { controller = "Welcome", action = "Welcome", id = "" });            
        }

        protected void Application_Start()
        {
            InitializeAutofac();

            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
			RegistrationExtensions.RegisterControllers(builder, Assembly.GetExecutingAssembly());
			RegistrationExtensions.RegisterModelBinders(builder, Assembly.GetExecutingAssembly());
            builder.RegisterType<QuestionRepository>().As<QuestionRepository>();
            builder.RegisterModule<AutofacCoreModule>();

            _containerProvider = new ContainerProvider(builder.Build());

            GlobalFilters.Filters.Add(new GlobalAuthorizationAttribute());
			ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(ContainerProvider));
        }
    }
}