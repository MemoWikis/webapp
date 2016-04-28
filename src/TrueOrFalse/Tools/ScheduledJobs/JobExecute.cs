using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            var jobHashCode = DateTime.Now.ToString().GetHashCode().ToString("x");

            CodeIsRunningInsideAJob = true;

            Settings.UseWebConfig = true;

            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName))
            {
                try
                {
                    ServiceLocator.AddScopeForCurrentThread(scope);

                    var stopwatch = Stopwatch.StartNew();

                    if (writeLog)
                        Logg.r().Information("JOB START: {Job}", jobName + " " + jobHashCode);

                    action(scope);

                    if (writeLog)
                        Logg.r()
                            .Information("JOB END: {Job} {timeNeeded}", jobName + " " + jobHashCode, stopwatch.Elapsed);

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