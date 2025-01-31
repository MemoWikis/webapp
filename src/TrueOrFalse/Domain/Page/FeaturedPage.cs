public class FeaturedPage
{
    public static readonly int RootPageId = Settings.FeaturedPageRootId;
    public static PageCacheItem Get => EntityCache.GetPage(Settings.FeaturedPageRootId);
    public static IReadOnlyList<int> LockedPageIds => GetLockedPageIds();

    public static bool Lockedpage(int pageId) =>
        LockedPageIds.Any(c => c == pageId);

    public static readonly int IntroPageId = 1864;
    public static readonly int MemoWikisWikiId = 1890;

    public static readonly IList<int> MainPageIds = new List<int> { 682, 687, 689, 709 };
    public static readonly IList<int> PopularPageIds = new List<int> { 269, 153, 266, 388, 680 };
    public static readonly IList<int> MemoWikisPageIds = new List<int> { 1876, 8975, 8974 };
    public static readonly IList<int> MemoWikisHelpIds = new List<int> { 1864, 9002 };

    private static IReadOnlyList<int> GetLockedPageIds()
    {
        var list = new List<int>();
        list.Add(RootPageId);
        list.AddRange(MainPageIds);
        list.Add(MemoWikisWikiId);
        list.AddRange(MemoWikisPageIds);
        list.AddRange(MemoWikisHelpIds);
        return list;
    }
}