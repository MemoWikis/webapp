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

            routes.MapRoute("Imprint-Route", "Imprint", new { controller = "Imprint", action = "Imprint" });
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
            builder.RegisterType<QuestionRepositoryDemoData>().As<IQuestionRepository>();
            builder.RegisterModule<AutofacCoreModule>();

            _containerProvider = new ContainerProvider(builder.Build());

			ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(ContainerProvider));
        }
    }
}