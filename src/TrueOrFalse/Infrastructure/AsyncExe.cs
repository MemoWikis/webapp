using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

public static class AsyncExe
{
    public static void Run(Action action, bool withAutofac = false)
    {
        try
        {
            Action actionExec;

            if (withAutofac)
            {
                actionExec = () =>
                {
                    Settings.UseWebConfig = true;
                    var container = AutofacWebInitializer.Run();
                    ServiceLocator.Init(container);

                    using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
                    {
                        action();
                    }
                };
            }
            else
            {
                actionExec = action;
            }

            if (ContextUtil.IsWebContext)
                HostingEnvironment.QueueBackgroundWorkItem(ct =>
                {
                    try
                    {
                        actionExec();
                    }
                    catch(Exception e)
                    {
                        Logg.r().Error(e, "Error in AsyncRunner in HostingEnvironment.QueueBackgroundWorkItem");
                    }
                    
                });
            else //for unit tests
                Task.Factory.StartNew(() => { actionExec(); });
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Error in AsyncRunner");
        }
    }
}