using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.String;

namespace TrueOrFalse.Tools;

public class IgnoreLog
{
    private static IEnumerable<string> _crawlers;

    public static bool ContainsCrawlerInHeader(string header, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        if (IsNullOrEmpty(header))
        {
            return false;
        }

        foreach (var crawlerName in GetCrawlers(httpContextAccessor, webHostEnvironment))
        {
            if (header.ToLower().IndexOf(crawlerName.Trim()) != -1)
            {
                return true;
            }
        }

        return false;
    }

    public static IEnumerable<string> GetCrawlers(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        if (_crawlers == null)
        {
            Initialize(httpContextAccessor, webHostEnvironment);
        }   

        return _crawlers;
    }

    public static void Initialize(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        var logIgnore = new PathTo(httpContextAccessor, webHostEnvironment).Log_Ignore(); 
        lock ("3fb23623-caed-48fc-6e86-c595b4c0820c")
        {
            if (!File.Exists(logIgnore))
            {
                Logg.r().Warning($"Ignore.log is not available- {logIgnore}");
                _crawlers = new List<string>();
            }

            var lines = File.ReadAllLines(logIgnore);

            _crawlers = lines
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .Select(line =>
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        return parts[1].ToLower().Trim();
                    }

                    return "";
                })
                .Where(crawlerName => !IsNullOrEmpty(crawlerName));
        }
    }

    public static void LoadNewList(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _crawlers = null;
        Initialize(httpContextAccessor, webHostEnvironment);
    }
}