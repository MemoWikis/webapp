using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Owin;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;

            var container = AutofacWebInitializer.Run(
                registerForAspNet: false, 
                registerForSignalR: true,
                assembly:Assembly.GetExecutingAssembly());
            
            hubConfiguration.Resolver = new AutofacDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.MapSignalR(hubConfiguration);
        }
    }    
}
