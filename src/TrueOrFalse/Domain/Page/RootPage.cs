public class RootPage
{
    public const int RootPageId = 1;
    public static PageCacheItem Get => EntityCache.GetPage(RootPageId);
    public static IReadOnlyList<int> LockedPageIds => GetLockedPageIds();

    public static bool Lockedpage(int pageId) =>
        LockedPageIds.Any(c => c == pageId);

    public const int IntroPageId = 1864;
    public const int MemoWikisWikiId = 1890;

    public static IList<int> MainPageIds = new List<int> { 682, 687, 689, 709 };
    public static IList<int> PopularPageIds = new List<int> { 269, 153, 266, 388, 680 };
    public static IList<int> MemoWikisPageIds = new List<int> { 1876, 8975, 8974 };
    public static IList<int> MemoWikisHelpIds = new List<int> { 1864, 9002 };

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