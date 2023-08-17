using System.Data;
using System.Diagnostics;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NHibernate;
using Serilog;

public class JobExecute
{
    [ThreadStatic]
    public static bool CodeIsRunningInsideAJob;

    public static void RunAsTask(Action<ILifetimeScope> action, string jobName, bool writeLog = true)
    {
        Task.Run(() => { Run(action, jobName, writeLog); });
    }

    public static void Run(Action<ILifetimeScope> action, string jobName, bool writeLog = true)
    {
        using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName))
        {
            var httpContextAccessor = scope.Resolve<IHttpContextAccessor>();
            var webHostEnvironment = scope.Resolve<IWebHostEnvironment>();
            try
            {
                try
                {
                    ServiceLocator.AddScopeForCurrentThread(scope);

                    CodeIsRunningInsideAJob = true;
                    Settings.UseWebConfig = true;

                    if (IsJobRunning(jobName,
                            scope, 
                            httpContextAccessor, 
                            webHostEnvironment))
                        return;

                    try
                    {
                        var logg = new Logg(httpContextAccessor, webHostEnvironment);

                        var stopwatch = Stopwatch.StartNew();

                        var threadId = Thread.CurrentThread.ManagedThreadId;
                        var appDomainName = AppDomain.CurrentDomain.FriendlyName;

                        if (writeLog)
                            logg.r()
                                 .Information("JOB START: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId}",
                                 jobName,
                                 appDomainName.GetHashCode().ToString("x"),
                                 threadId);

                        action(scope);

                        if (writeLog)
                            logg.r()
                                .Information("JOB END: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId}, {timeNeeded}",
                                    jobName,
                                    appDomainName.GetHashCode().ToString("x"),
                                    threadId,
                                    stopwatch.Elapsed);

                        stopwatch.Stop();
                    }
                    finally
                    {
                        CloseJob(jobName, scope, httpContextAccessor, webHostEnvironment);
                    }
                }
                finally
                {
                    CodeIsRunningInsideAJob = false;
                    ServiceLocator.RemoveScopeForCurrentThread();
                }

            }
            catch (Exception e)
            {
                new Logg(httpContextAccessor, webHostEnvironment).r().Error(e, "Job error on {JobName}", jobName);
            }
        }
    }

    private static bool IsJobRunning(string jobName, 
        ILifetimeScope scope, 
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment)
    {
        using (new MutexX(5000, "IsRunning"))
        {
            using (var session = scope.Resolve<ISessionBuilder>().OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.Serializable))
            {
                transaction.Begin();
                var runningJobRepo = new RunningJobRepo(session, httpContextAccessor, webHostEnvironment);
                if (runningJobRepo.IsJobRunning(jobName))
                {
                    new Logg(httpContextAccessor, webHostEnvironment)
                        .r()
                        .Information("Job is already running: {jobName}, {Environment}", 
                        jobName,
                        Settings.Environment(httpContextAccessor.HttpContext, webHostEnvironment));

                    return true;
                }

                runningJobRepo.Add(jobName);
                transaction.Commit();
            }

            return false;
        }
    }

    private static void CloseJob(string jobName,
        ILifetimeScope scope, 
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment)
    {
        using (var session = scope.Resolve<ISessionBuilder>().OpenSession())
        {
            new RunningJobRepo(session, httpContextAccessor, webHostEnvironment).Remove(jobName);
        }
    }
}