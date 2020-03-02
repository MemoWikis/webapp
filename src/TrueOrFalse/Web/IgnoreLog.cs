﻿using System.Collections.Generic;
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

             var path = @"C:\memucho\webapp\src\TrueOrFalse.Frontend.Web\Log.Ignore";

            if(!File.Exists(path))
                Logg.r().Warning($"Ignore.log is not available- {path}");
            else
            {
                var lines = File.ReadAllLines(path);

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

    }
}
