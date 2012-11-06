using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using Module = Autofac.Module;

namespace TrueOrFalse.Tests
{
    public class AutofacTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("TrueOrFalse.Tests")).AssignableTo<IRegisterAsInstancePerLifetime>();

        }
    }
}
