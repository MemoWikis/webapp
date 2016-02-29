using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using RollbarSharp;

public class JobExecute
{
    public static void Run(Action<ILifetimeScope> action, bool writeLog = true)
    {
        try
        {
            var jobName = action.GetMethodInfo().DeclaringType?.Name;

            Settings.UseWebConfig = true;
            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
            {
                var stopwatch = Stopwatch.StartNew();

                if(writeLog)
                    Logg.r().Information("JOB START: {Job}", jobName);

                action(scope);

                if (writeLog)
                    Logg.r().Information("JOB END: {Job} {timeNeeded}", jobName, stopwatch.Elapsed);

                stopwatch.Stop();
            }
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Job error");
            new RollbarClient().SendException(e);
        }
    }
}