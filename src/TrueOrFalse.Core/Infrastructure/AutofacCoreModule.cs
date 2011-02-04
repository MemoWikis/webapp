using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace TrueOrFalse.Core.Infrastructure
{
    public class AutofacCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuestionRepository>();
            builder.RegisterInstance(SessionFactory.CreateSessionFactory().OpenSession());

        }
    }
}
