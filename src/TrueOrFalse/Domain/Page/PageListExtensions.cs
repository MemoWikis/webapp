public static class PageListExtensions
{
    public static Page ByName(this IEnumerable<Page> categories, string name) =>
        categories.First(c => c.Name == name);

    public static PageCacheItem ByName(
        this IEnumerable<PageCacheItem> categories,
        string name) =>
        categories.First(c => c.Name == name);

    public static IEnumerable<int> GetIds(this IEnumerable<PageCacheItem> sets)
    {
        var ids = sets.Select(q => q.Id).ToList();
        return ids;
    }
}