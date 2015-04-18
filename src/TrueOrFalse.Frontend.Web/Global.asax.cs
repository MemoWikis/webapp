using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using RollbarSharp;
using StackExchange.Profiling;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.View;
using TrueOrFalse.Web.JavascriptView;

namespace TrueOrFalse.Frontend.Web
{
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

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new JavaScriptViewEngine());
            ViewEngines.Engines.Add(new PartialSubDirectoriesViewEngine());
            
            JobScheduler.Start();
        }


        private void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
#if DEBUG
            MiniProfiler.Start();
#endif
        }

        protected void Application_EndRequest()
        {
#if DEBUG
            MiniProfiler.Stop();
#endif
        }

        private void InitializeAutofac()
        {
            DependencyResolver.SetResolver(
                new AutofacDependencyResolver(
                    AutofacWebInitializer.Run(
                        registerForAspNet: true, 
                        assembly:Assembly.GetExecutingAssembly()
                    )));
        }

        protected void Session_Start()
        {
            if(!Sl.Resolve<SessionUser>().IsLoggedIn)
                Sl.Resolve<LoginFromCookie>().Run();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            if (Request.IsLocal)
            {
                return;
            }

            var exception = Server.GetLastError();
            if (exception == null)
                return;

            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

            if (code != 404)
            {
                try
                {
                    (new RollbarClient()).SendException(exception);
                }
                catch{}
            }

            Response.Clear();
            Server.ClearError();

            Response.Redirect(string.Format("~/Fehler/{0}", code), false);
        }
    }
}