using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrueOrFalse.Tools
{
   public class Ignore
    {
        public static IList<string> CrawlerIgnoreList = new List<string>();
        

        public static void GetCrawlerList()
        {
            var path = @"C:\memucho\webapp\src\TrueOrFalse.Frontend.Web\Ignore.log";

            if(!File.Exists(path))
                Logg.r().Warning($"Ignore.log is not available- {path}");
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        CrawlerIgnoreList.Add(sr.ReadLine().Trim());
                    }
                }
            }
        }

        public static void  ThreadSafeReload()
        {
           var thread2 = new Thread(new ThreadStart(GetCrawlerList));
           thread2.Start();
        }

    }
}
