using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using RollbarSharp;

public class JobExecute
{
    [ThreadStatic] public static bool CodeIsRunningInsideAJob;

    public static void Run(Action<ILifetimeScope> action, string jobName, bool writeLog = true)
    {
        var runningJobRepo = Sl.R<RunningJobRepo>();

        try
        {
            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName))
            {
                try
                {
                    ServiceLocator.AddScopeForCurrentThread(scope);

                    CodeIsRunningInsideAJob = true;
                    Settings.UseWebConfig = true;

                    try
                    {
                        using (new MutexX(5000, "IsRunning"))
                        {
                            if (runningJobRepo.IsJobRunning(jobName))
                            {
                                Logg.r().Information("Job is already running: {jobName}", jobName);
                                return;
                            }

                            runningJobRepo.Add(jobName);
                        }

                        var stopwatch = Stopwatch.StartNew();

                        var threadId = Thread.CurrentThread.ManagedThreadId;
                        var appDomainName = AppDomain.CurrentDomain.FriendlyName;

                        if (writeLog)
                            Logg.r()
                                .Information("JOB START: {Job} {AppDomain} {ThreadId}", jobName, appDomainName, threadId);

                        action(scope);

                        if (writeLog)
                            Logg.r()
                                .Information("JOB END: {Job} {AppDomain} {ThreadId} {timeNeeded}", jobName,
                                    appDomainName,
                                    threadId, stopwatch.Elapsed);

                        stopwatch.Stop();
                    }
                    finally
                    {
                        runningJobRepo.Remove(jobName);
                    }
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
    }
}