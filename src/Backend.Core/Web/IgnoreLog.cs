using static System.String;

public class IgnoreLog
{
    private static IEnumerable<string>? _crawlers;

    public static bool ContainsCrawlerInHeader(string header)
    {
        if (IsNullOrEmpty(header))
        {
            return false;
        }

        foreach (var crawlerName in GetCrawlers())
        {
            if (header.ToLower().IndexOf(crawlerName.Trim()) != -1)
                return true;
        }

        return false;
    }

    public static IEnumerable<string>? GetCrawlers()
    {
        if (_crawlers == null) 
            Initialize();

        return _crawlers;
    }

    public static void Initialize()
    {
        var logIgnorePath = PathTo.Log_Ignore(); 
        lock ("3fb23623-caed-48fc-6e86-c595b4c0820c")
        {
            if (!File.Exists(logIgnorePath))
            {
                Logg.r.Warning($"Ignore.log is not available- {logIgnorePath}");
                _crawlers = new List<string>();
            }

            var lines = File.ReadAllLines(logIgnorePath);

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

    public static void LoadNewList()
    {
        _crawlers = null;
        Initialize();
    }
}