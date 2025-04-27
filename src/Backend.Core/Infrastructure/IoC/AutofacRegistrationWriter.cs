using System.Reflection;
using System.Text;
using NHibernate.Util;

public class AutofacRegistrationWriter
{
    public static string Run()
    {
        var assemblyWeb = Assembly.Load("MemoWikis.Backend.Host");
        var assemblyDomain = Assembly.Load("MemoWikis.Backend.Core");

        var type = typeof(IRegisterAsInstancePerLifetime);
        var sb = new StringBuilder();
        new List<Assembly> { assemblyWeb, assemblyDomain }
            .SelectMany(x => x.GetTypes())
            .Where(x =>
                (type.IsAssignableFrom(x) && !x.IsInterface) ||
                x.Name.EndsWith("Repository") || x.Name.EndsWith("Repo"))
            .ForEach(x =>
                sb.AppendLine("builder.RegisterType<" + x.Name + ">().InstancePerLifetimeScope();")
            );

        return sb.ToString();
    }
}