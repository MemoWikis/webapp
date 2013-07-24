using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.SolrNet;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.View;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Web.JavascriptView;
using TrueOrFalse.Updates;

namespace TrueOrFalse.Frontend.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Global : HttpApplication
    {
        
        protected void Application_Start()
        {
            InitializeAutofac();
#if DEBUG
            NHibernateProfiler.Initialize();
#endif
            
            Sl.Resolve<Update>().Run();

            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles); 
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Add(new JavaScriptViewEngine());
            GlobalFilters.Filters.Add(new GlobalAuthorizationAttribute());
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModule<AutofacCoreModule>();
            builder.RegisterModule(new SolrNetModule(WebConfigSettings.SolrUrl + "tofQuestion"));

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Session_Start()
        {
            if(!Sl.Resolve<SessionUser>().IsLoggedIn)
                Sl.Resolve<LoginFromCookie>().Run();
        }
    }
}