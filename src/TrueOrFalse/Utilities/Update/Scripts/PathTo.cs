using System;
using System.IO;
using System.Web;

namespace TrueOrFalse.Updates
{
    public class PathTo
    {
        public static string Scrips(string fileName)
        {
            return "Utilities/Update/Scripts/" + fileName;
        }

        public static string SolrSchema(string fileName)
        {
            if(HttpContext.Current != null)
                return HttpContext.Current.Server.MapPath("bin/Utilities/Update/SolrSchemas/" + fileName);

            return AppDomain.CurrentDomain.BaseDirectory + "/Utilities/Update/SolrSchemas/" + fileName;
        }
    }
}
