using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Updates
{
    public class SolrCoreReload
    {
        public static void ReloadCategory(){Run("Category");}
        public static void ReloadQuestion() { Run("Question"); }
        public static void ReloadSet() { Run("Set"); }
        public static void ReloadUser() { Run("User"); }

        public static void ReloadAll()
        {
            ReloadCategory();
            ReloadQuestion();
            ReloadSet();
            ReloadUser();
        }

        public static void Run(string coreName)
        {
            CopySchema(coreName);

            var url = WebConfigSettings.SolrPath + "admin/cores?wt=json&action=RELOAD&core=tof" + coreName;
            WebRequest.Create(url).GetResponse();
        }

        private static void CopySchema(string coreName)
        {
            File.Copy(
                PathTo.SolrSchema("schema" + coreName + ".xml"),
                WebConfigSettings.SolrPath + "/tof" + coreName + "/conf/" + "schema.xml",
                overwrite:true
            );
        }
    }
}
