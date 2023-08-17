using Autofac;
using TrueOrFalse.Infrastructure;

public class AsyncExe
{
    private readonly ILifetimeScope _lifetimeScope;

    public AsyncExe(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public void Run(Action action, bool withAutofac = false)
    {
        try
        {
            Action actionExec;

            if (withAutofac)
            {
                actionExec = () =>
                {
                    Settings.UseWebConfig = true;
                    using (var scope = _lifetimeScope.BeginLifetimeScope())
                    {
                        action();
                    }
                };
            }
            else
            {
                actionExec = action;
            }

            try
            {
                Task.Factory.StartNew(() => { actionExec(); });
            }
            catch (Exception e)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in AsyncRunner in HostingEnvironment.QueueBackgroundWorkItem");
            }
        }
        catch (Exception e)
        {
            new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in AsyncRunner");
        }
    }
}