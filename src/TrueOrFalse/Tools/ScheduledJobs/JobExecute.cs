using System;
using Autofac;
using RollbarSharp;

public class JobExecute
{
    public static void Run(Action<ILifetimeScope> action)
    {
        try
        {
            Settings.UseWebConfig = true;
            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
            {
                action(scope);
            }
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Job error");
            new RollbarClient().SendException(e);
        }
    }    
}