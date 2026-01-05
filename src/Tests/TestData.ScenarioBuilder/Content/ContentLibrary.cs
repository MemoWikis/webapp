using System.Text.Json;
using Serilog;

/// <summary>
/// Loads wiki content from JSON files organized by theme.
/// </summary>
public sealed class ContentLibrary
{
    private readonly string _contentPath;
    private readonly Dictionary<string, List<WikiContentDefinition>> _cache = new();

    public ContentLibrary(string? contentBasePath = null)
    {
        _contentPath = contentBasePath ?? Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Content");
    }

    /// <summary>
    /// Gets all wikis for a specific theme (e.g., "Politics", "History", "Technology").
    /// </summary>
    public List<WikiContentDefinition> GetWikisForTheme(string theme)
    {
        var normalizedTheme = theme.ToLowerInvariant();

        // Check cache first
        if (_cache.TryGetValue(normalizedTheme, out var cached))
            return cached;

        var themePath = Path.Combine(_contentPath, normalizedTheme);

        if (!Directory.Exists(themePath))
        {
            Log.Warning("Content directory not found for theme: {Theme} at {Path}", theme, themePath);
            return new List<WikiContentDefinition>();
        }

        var files = Directory.GetFiles(themePath, "*.json");
        var wikis = new List<WikiContentDefinition>();

        foreach (var file in files)
        {
            try
            {
                var json = File.ReadAllText(file);
                var wiki = JsonSerializer.Deserialize<WikiContentDefinition>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (wiki != null)
                    wikis.Add(wiki);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load wiki content from file: {File}", file);
            }
        }

        _cache[normalizedTheme] = wikis;
        return wikis;
    }

    /// <summary>
    /// Gets all available themes (directories in Content folder).
    /// </summary>
    public List<string> GetAvailableThemes()
    {
        if (!Directory.Exists(_contentPath))
            return new List<string>();

        return Directory.GetDirectories(_contentPath)
            .Select(Path.GetFileName)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToList()!;
    }

    /// <summary>
    /// Gets a specific number of random wikis for a theme.
    /// </summary>
    public List<WikiContentDefinition> GetRandomWikisForTheme(string theme, int count, Random random)
    {
        var allWikis = GetWikisForTheme(theme);

        if (allWikis.Count == 0)
            return new List<WikiContentDefinition>();

        return allWikis
            .OrderBy(_ => random.Next())
            .Take(Math.Min(count, allWikis.Count))
            .ToList();
    }
}
