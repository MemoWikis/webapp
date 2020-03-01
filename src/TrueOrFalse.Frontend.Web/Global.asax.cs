using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using StackExchange.Profiling;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.View;
using TrueOrFalse.Tools;
using TrueOrFalse.Web.JavascriptView;

namespace TrueOrFalse.Frontend.Web
{
    public class Global : HttpApplication
    {        
       
        protected void Application_Start()
        {
            Ignore.GetCrawlerList();
            InitializeAutofac();
            
            Sl.Resolve<Update>().Run();

            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles); 
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new JavaScriptViewEngine());
            ViewEngines.Engines.Add(new PartialSubDirectoriesViewEngine());

            if (!Settings.DisableAllJobs())
                JobScheduler.Start();

            if (Settings.InitEntityCacheViaJobScheduler())
            {
                JobScheduler.StartImmediately_RefreshEntityCache();//Is a lot faster (for unknown reasons) than direct init but bears the risk of EntityCache not being filled before first request
            }
            else
            {
                EntityCache.Init();
            }



            Logg.r().Information("=== Application Start ===============================");
        }

        protected void Application_Stop()
        {
            JobScheduler.Shutdown();
            Logg.r().Information("=== Application Stop ===============================");
        }

        private void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
#if DEBUG
            if (Settings.DebugEnableMiniProfiler())
                MiniProfiler.Start();
#endif
        }

        protected void Application_EndRequest()
        {
#if DEBUG
            if (Settings.DebugEnableMiniProfiler())
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
                LoginFromCookie.Run();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

            if (code != 404)
                Logg.Error(exception);

            if(!Request.IsLocal)
                Response.Redirect(string.Format("~/Fehler/{0}", code), true);
        }
    }
}