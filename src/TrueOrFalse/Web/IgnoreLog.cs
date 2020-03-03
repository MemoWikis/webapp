using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static System.String;

namespace TrueOrFalse.Tools
{
    public class IgnoreLog
    {
        private static IEnumerable<string> _crawlers;

        public static IEnumerable<string> GetCrawlers()
        {
            lock ("3fb23623-caed-48fc-6e86-c595b4c0820c")
            {
                if (_crawlers != null)
                    return _crawlers;
            }

            if (!File.Exists(PathTo.Log_Ignore()))
            {
                Logg.r().Warning($"Ignore.log is not available- {PathTo.Log_Ignore()}");
                return new List<string>();
            }

            var lines = File.ReadAllLines(PathTo.Log_Ignore());

            return _crawlers = lines
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .Select(line =>
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                        return parts[1].ToLower().Trim();
                    return "";
                })
                .Where(crawlerName => !IsNullOrEmpty(crawlerName));
        }

        public static bool ContainsCrawlerInHeader(string header)
        {
            if (IsNullOrEmpty(header))
                return false;

            foreach (var crawlerName in GetCrawlers())
            {
                if (header.ToLower().IndexOf(crawlerName.Trim()) != -1)
                    return true;
            }
            return false;
        }
        public static void LoadNewList()
        {
            _crawlers = null;
            GetCrawlers();
        }
    }
}

