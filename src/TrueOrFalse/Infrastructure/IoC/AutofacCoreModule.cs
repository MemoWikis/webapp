using System;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using NHibernate;
using TrueOrFalse.Infrastructure.Persistence;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace TrueOrFalse.Infrastructure
{
    public class AutofacCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web"))
                   .AssignableTo<IRegisterAsInstancePerLifetime>();
            
            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse");
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse)
                .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Repo"));

            try
            {
                builder.RegisterInstance(SessionFactory.CreateSessionFactory());
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    if (innerException is ReflectionTypeLoadException)
                        break;

                    innerException = innerException.InnerException;
                }

                if (innerException == null)
                    throw;

                foreach (Exception exSub in ((ReflectionTypeLoadException)innerException).LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }

                throw new Exception(sb.ToString());
            }

            builder.Register(context => new SessionManager(context.Resolve<ISessionFactory>().OpenSession())).InstancePerLifetimeScope();
            builder.Register(context => context.Resolve<SessionManager>().Session).ExternallyOwned();
        }
    }
}