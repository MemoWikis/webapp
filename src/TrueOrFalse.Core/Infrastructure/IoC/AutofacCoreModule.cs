using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Module = Autofac.Module;

namespace TrueOrFalse.Core.Infrastructure
{
    public class AutofacCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.View.Web")).AssignableTo<IRegisterAsInstancePerLifetime>();
            
            var assemblyTrueOrFalse = Assembly.Load("TrueOrFalse.Core");
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).AssignableTo<IRegisterAsInstancePerLifetime>();
            builder.RegisterAssemblyTypes(assemblyTrueOrFalse).Where(a => a.Name.EndsWith("Repository"));

            builder.RegisterType<QuestionRepository>();
            builder.RegisterInstance(SessionFactory.CreateSessionFactory().OpenSession());
        }
    }
}
