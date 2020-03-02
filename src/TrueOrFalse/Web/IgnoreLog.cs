using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.String;

namespace TrueOrFalse.Tools
{
   public class IgnoreLog
    {
        public static IEnumerable<string> GetCrawlers()
        {

            IEnumerable<string> crawlerNames = new List<string>();
            if(!File.Exists(PathTo.Log_Ignore()))
                Logg.r().Warning($"Ignore.log is not available- {PathTo.Log_Ignore()}");
            else
            {
                var lines = File.ReadAllLines(PathTo.Log_Ignore());

                crawlerNames = lines
                    .Select(line => line.Trim())
                    .Where(line => line.Length > 0)
                    .Select(line =>
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2)
                            return parts[1];
                        return "";
                    })
                    .Where(crawlerName => !IsNullOrEmpty(crawlerName));
            }
            return crawlerNames;
        }

        public static void LoadNewList()
        {
            lock ("3fb23623-caed-48fc-6e86-c595b4c0820c")
            {
               GetCrawlers();
            }
           
        }
        
    }
}
