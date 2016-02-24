using System.IO;
using System.Web;
using NHibernate;

public class ExecuteSqlFile 
{
    public static void Run(string filePath)
    {
        if (HttpContext.Current != null)
            filePath = HttpContext.Current.Server.MapPath("bin/" + filePath);

        Sl.R<ISession>().CreateSQLQuery(File.ReadAllText(filePath)).ExecuteUpdate();    
    }
}