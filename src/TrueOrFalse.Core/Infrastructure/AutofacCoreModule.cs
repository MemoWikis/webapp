using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using TrueOrFalse.Tests.Answer;

namespace TrueOrFalse.Core.Infrastructure
{
    public class AutofacCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuestionService>();
            builder.RegisterInstance(SessionFactory.CreateSessionFactory().OpenSession());
           
        }
    }
}
