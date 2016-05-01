using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autofac;
using RollbarSharp;

public class JobExecute
{
    [ThreadStatic]
    public static bool CodeIsRunningInsideAJob;

    private static readonly IList<string> _runningJobs = new List<string>();

    private static readonly object _lockObject1 = new object();
    private static readonly object _lockObject2 = new object();

    public static void Run(Action<ILifetimeScope> action, string jobName, bool writeLog = true)
    {
        try
        {
            lock (_lockObject1)
            {
                if (_runningJobs.Any(x => x == jobName))
                {
                    Logg.r().Information("Job is already running: {jobName}", jobName);
                    return;
                }

                _runningJobs.Add(jobName);
            }


            CodeIsRunningInsideAJob = true;

            Settings.UseWebConfig = true;

            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName))
            {
                try
                {
                    ServiceLocator.AddScopeForCurrentThread(scope);

                    var stopwatch = Stopwatch.StartNew();

                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var appDomainName = AppDomain.CurrentDomain.FriendlyName;

                    if (writeLog)
                        Logg.r().Information("JOB START: {Job} {AppDomain} {ThreadId}", jobName, appDomainName, threadId);

                    action(scope);

                    if (writeLog)
                        Logg.r()
                            .Information("JOB END: {Job} {AppDomain} {ThreadId} {timeNeeded}", jobName, appDomainName, threadId, stopwatch.Elapsed);

                    stopwatch.Stop();
                }
                finally
                {
                    ServiceLocator.RemoveScopeForCurrentThread();
                }
            }
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Job error on " + jobName);
            new RollbarClient().SendException(e);
        }

        finally
        {
            lock (_lockObject2)
            {
                _runningJobs.Remove(jobName);
            }
        }
    }
}