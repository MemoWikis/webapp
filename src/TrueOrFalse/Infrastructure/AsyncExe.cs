using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

public class AsyncExe
{
    public static void Run(Action action)
    {
        try
        {
            var container = AutofacWebInitializer.Run();
            ServiceLocator.Init(container);

            Action actionExec = () =>
            {
                using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope())
                {
                    action();
                }
            };

            if (ContextUtil.IsWebContext)
                HostingEnvironment.QueueBackgroundWorkItem(ct => { actionExec(); });
            else
                Task.Factory.StartNew(() => { actionExec(); });
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Error in AsyncRunner");
        }
    }
}