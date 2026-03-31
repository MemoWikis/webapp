public static class PublicWikiCache
{
    private static readonly object _lock = new();
    private static List<PageCacheItem> _publicWikis = new();

    public static IReadOnlyList<PageCacheItem> GetAllPublicWikis()
    {
        lock (_lock)
        {
            return _publicWikis.ToList();
        }
    }

    public static void Initialize(IEnumerable<PageCacheItem> allPages)
    {
        lock (_lock)
        {
            _publicWikis = allPages
                .Where(p => p.IsWiki && p.IsPublic)
                .ToList();
        }
    }

    public static void Add(PageCacheItem page)
    {
        if (!page.IsWiki || !page.IsPublic)
        {
            return;
        }

        lock (_lock)
        {
            if (_publicWikis.All(w => w.Id != page.Id))
            {
                _publicWikis.Add(page);
            }
        }
    }

    public static void Remove(int pageId)
    {
        lock (_lock)
        {
            _publicWikis.RemoveAll(w => w.Id == pageId);
        }
    }

    public static void UpdateIfRelevant(PageCacheItem page)
    {
        lock (_lock)
        {
            var existingIndex = _publicWikis.FindIndex(w => w.Id == page.Id);
            var shouldBeInCache = page.IsWiki && page.IsPublic;

            if (shouldBeInCache && existingIndex >= 0)
            {
                _publicWikis[existingIndex] = page;
            }
            else if (shouldBeInCache && existingIndex < 0)
            {
                _publicWikis.Add(page);
            }
            else if (!shouldBeInCache && existingIndex >= 0)
            {
                _publicWikis.RemoveAt(existingIndex);
            }
        }
    }
}
