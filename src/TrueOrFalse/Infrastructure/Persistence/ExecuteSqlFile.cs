using System.IO;
using System.Web;
using NHibernate;
using TrueOrFalse;

public class ExecuteSqlFile : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public ExecuteSqlFile(ISession session){
        _session = session;
    }

    public void Run(string filePath)
    {
        if (HttpContext.Current != null)
            filePath = HttpContext.Current.Server.MapPath("bin/" + filePath);

        _session.CreateSQLQuery(File.ReadAllText(filePath)).ExecuteUpdate();    
    }
}