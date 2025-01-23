﻿using Autofac;
using NHibernate;
using System.Data;
using System.Diagnostics;

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
        using var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName);
        try
        {
            try
            {
                ServiceLocator.AddScopeForCurrentThread(scope);

                CodeIsRunningInsideAJob = true;
                Settings.UseWebConfig = true;

                if (IsJobRunning(jobName, scope))
                    return;

                try
                {
                    var stopwatch = Stopwatch.StartNew();

                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var appDomainName = AppDomain.CurrentDomain.FriendlyName;

                    if (writeLog)
                        Logg.r.Information("JOB START: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId}",
                            jobName,
                            appDomainName.GetHashCode().ToString("x"),
                            threadId
                        );

                    action(scope);

                    if (writeLog)
                        Logg.r.Information("JOB END: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId}, {timeNeeded}",
                            jobName,
                            appDomainName.GetHashCode().ToString("x"),
                            threadId,
                            stopwatch.Elapsed
                        );

                    stopwatch.Stop();
                }
                finally
                {
                    CloseJob(jobName, scope);
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
            Logg.r.Error(e, "Job error on {JobName}", jobName);
        }
    }

    private static bool IsJobRunning(string jobName, ILifetimeScope scope)
    {
        using (new MutexX(5000, "IsRunning"))
        {
            using (var session = scope.Resolve<ISessionBuilder>().OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.Serializable))
            {
                transaction.Begin();
                var runningJobRepo = new RunningJobRepo(session);
                if (runningJobRepo.IsJobRunning(jobName))
                {
                    Logg.r.Information("Job is already running: {jobName}, {Environment}",
                        jobName,
                        Settings.Environment);

                    return true;
                }

                runningJobRepo.Add(jobName);
                transaction.Commit();
            }

            return false;
        }
    }

    private static void CloseJob(string jobName,
        ILifetimeScope scope)
    {
        using (var session = scope.Resolve<ISessionBuilder>().OpenSession())
        {
            new RunningJobRepo(session).Remove(jobName);
        }
    }
}