using MessagePack;

[MessagePackObject]
public record struct PageViewSummaryWithId(
    [property: Key(0)] Int64 Count,
    [property: Key(1)] DateTime DateOnly,
    [property: Key(2)] int PageId,
    [property: Key(3)] DateTime LastPageViewCreatedAt);

public class PageViewMmapCache : MmapCacheBase<PageViewSummaryWithId>, IRegisterAsInstancePerLifetime
{
    public const int SchemaVersion = 1;

    protected override int SchemaVersionValue => SchemaVersion;

    public PageViewMmapCache() : base("pageviews.mmap", "PageView") { }

    public List<PageViewSummaryWithId> LoadPageViews() => Load();

    public void SaveAllPageViews(IList<PageViewSummaryWithId> views) => SaveAll(views);

    public void AppendPageView(PageViewSummaryWithId view) =>
        AppendSingle(view, $"PageId={view.PageId}, DateOnly={view.DateOnly}");

    public void AppendPageViews(IEnumerable<PageViewSummaryWithId> views) => AppendRange(views);

    public void DeletePageViews(int pageId) =>
        DeleteWhere(v => v.PageId == pageId, $"PageId={pageId}");
}