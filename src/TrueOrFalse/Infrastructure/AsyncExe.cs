using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrueOrFalse.Infrastructure;

public class AsyncExe
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AsyncExe(ILifetimeScope lifetimeScope,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _lifetimeScope = lifetimeScope;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
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
                Logg.r.Error(e, "Error in AsyncRunner in HostingEnvironment.QueueBackgroundWorkItem");
            }
        }
        catch (Exception e)
        {
            Logg.r.Error(e, "Error in AsyncRunner");
        }
    }
}