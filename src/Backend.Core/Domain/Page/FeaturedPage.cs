public class FeaturedPage
{
    public static readonly int RootPageId = Settings.FeaturedPageRootId;
    public static PageCacheItem GetRootPage => EntityCache.GetPage(Settings.FeaturedPageRootId);
    public static IReadOnlyList<int> LockedPageIds => GetLockedPageIds();

    public static bool Lockedpage(int pageId) =>
        LockedPageIds.Any(c => c == pageId);

    public static readonly int IntroPageId = Settings.FeaturedPageIntroId;
    public static readonly int MemoWikisWikiId = Settings.FeaturedPageMemoWikisWikiId;

    public static readonly IList<int> MainPageIds = Settings.FeaturedMainPageIds;
    public static readonly IList<int> PopularPageIds = Settings.FeaturedPopularPageIds;
    public static readonly IList<int> MemoWikisPageIds = Settings.FeaturedMemoWikisPageIds;
    public static readonly IList<int> MemoWikisHelpIds = Settings.FeaturedMemoWikisHelpIds;

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