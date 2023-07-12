using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using Serilog;
using StackExchange.Profiling;
using Stripe;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Tools;
using TrueOrFalse.Updates;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace TrueOrFalse.Frontend.Web;

public class Global : HttpApplication
{
    protected void Application_EndRequest(object source, EventArgs e)
    {
#if DEBUG
        //if (Settings.DebugEnableMiniProfiler())
        //MiniProfiler.Current.Stop();

        var app = (HttpApplication)source;
        var uriObject = app.Context.Request.Url;

        if (!uriObject.AbsolutePath.Contains("."))
        {
            var elapsed = ((Stopwatch)app.Context.Items["requestStopwatch"]).Elapsed;
            Log.Information("=== End Request: {pathAndQuery} {elapsed}==", uriObject.PathAndQuery, elapsed);
        }
#endif
    }

    protected void Application_Error(object sender, EventArgs e)
    {
        var exception = Server.GetLastError();
        if (exception == null)
        {
            return;
        }

        var code = exception is HttpException ? (exception as HttpException).GetHttpCode() : 500;

        if (code != 404)
        {
            Logg.Error(exception);
        }

        if (!Request.IsLocal)
        {
            Response.Redirect(string.Format("~/Fehler/{0}", code), true);
        }
    }

    protected void Application_Start()
    {
        Logg.r().Information("=== Application Start (start) ===============================");
        InitializeAutofac();
        var container = AutofacWebInitializer.Run(registerForAspNet: true, assembly: Assembly.GetExecutingAssembly());
        using (var scope = container.BeginLifetimeScope())
        {
            var categoryRepo = scope.Resolve<CategoryRepository>();
            var questionRepo = scope.Resolve<QuestionRepo>();
            var userRepo = scope.Resolve<UserRepo>();
            var update = scope.Resolve<Update>();
            var nhibernateSession = scope.Resolve<ISession>();
            var runningJobRepo = scope.Resolve<RunningJobRepo>();

            StripeConfiguration.ApiKey = Settings.SecurityKeyStripe;
            IgnoreLog.Initialize();
            
            update.Run();

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new PartialSubDirectoriesViewEngine());

            if (!Settings.DisableAllJobs())
            {
                JobScheduler.Start(runningJobRepo);
            }

            if (Settings.InitEntityCacheViaJobScheduler())
            {
                JobScheduler
                    .StartImmediately_RefreshEntityCache();
            }
            else
            {
                var stopwatch = Stopwatch.StartNew();
                Logg.r().Information("=== Init EntityCache (start) ===============================");


           
                new EntityCacheInitializer(categoryRepo, questionRepo, userRepo).Init();


                Logg.r().Information(
                    $"=== Init EntityCache (end, elapsed {stopwatch.Elapsed}) ===============================");
                stopwatch.Stop();
            }

            nhibernateSession.Close();
        }

        Logg.r().Information("=== Application Start (end) ===============================");
    }

    protected void Application_Stop()
    {
        JobScheduler.Shutdown();
        Logg.r().Information("=== Application Stop ===============================");
    }

    protected void Session_Start()
    {
        var container = AutofacWebInitializer.Run(registerForAspNet: true, assembly: Assembly.GetExecutingAssembly());
        using (var scope = container.BeginLifetimeScope())
        {
            var sessionUser = scope.Resolve<SessionUser>();
            var userRepo = scope.Resolve<UserRepo>();
            var persistentLoggingRepo =  scope.Resolve<PersistentLoginRepo>();

            if (!sessionUser.IsLoggedIn)
            {
                LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userRepo);
            }
        }
    }

    private void Application_BeginRequest(object source, EventArgs e)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
#if DEBUG
        if (Settings.DebugEnableMiniProfiler())
        {
            MiniProfiler.StartNew();
        }

        var app = (HttpApplication)source;
        var uriObject = app.Context.Request.Url;

        if (!uriObject.AbsolutePath.Contains("."))
        {
            app.Context.Items.Add("requestStopwatch", Stopwatch.StartNew());
            Log.Information("=== Start Request: {pathAndQuery} ==", uriObject.PathAndQuery);
        }
#endif
    }

    private void InitializeAutofac()
    {
        DependencyResolver.SetResolver(
            new AutofacDependencyResolver(
                AutofacWebInitializer.Run(
                    true,
                    Assembly.GetExecutingAssembly()
                )));
    }
}