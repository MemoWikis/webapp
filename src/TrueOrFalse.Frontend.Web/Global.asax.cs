using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Builder;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using TrueOrFalse.Tests.Answer;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        static IContainerProvider _containerProvider;

        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

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
            builder.RegisterModule(new AutofacControllerModule(Assembly.GetExecutingAssembly()));
            builder.Register<QuestionServiceDemoData>().As<IQuestionService>();

            _containerProvider = new ContainerProvider(builder.Build());
            
            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(ContainerProvider));
        }
    }
}