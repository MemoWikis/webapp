using MessagePack;

[MessagePackObject]
public record struct PageChangeSummary(
    [property: Key(0)] int Id,
    [property: Key(1)] int PageId,
    [property: Key(2)] int DataVersion,
    [property: Key(3)] string Data,
    [property: Key(4)] int AuthorId,
    [property: Key(5)] int Type,
    [property: Key(6)] DateTime DateCreated);

public class PageChangeMmapCache : MmapCacheBase<PageChangeSummary>, IRegisterAsInstancePerLifetime
{
    public const int SchemaVersion = 1;

    protected override int SchemaVersionValue => SchemaVersion;

    public PageChangeMmapCache() : base("pagechanges.mmap", "PageChange") { }

    public List<PageChangeSummary> LoadPageChanges() => Load();

    public void SaveAllPageChanges(IList<PageChangeSummary> changes) => SaveAll(changes);

    public static IList<PageChange> ToPageChanges(List<PageChangeSummary> summaries)
    {
        return summaries.Select(s => new PageChange
        {
            Id = s.Id,
            Page = new Page { Id = s.PageId },
            DataVersion = s.DataVersion,
            Data = s.Data,
            AuthorId = s.AuthorId,
            Type = (PageChangeType)s.Type,
            DateCreated = s.DateCreated
        }).ToList();
    }

    public static IList<PageChangeSummary> FromPageChanges(IList<PageChange> changes)
    {
        return changes.Select(c => new PageChangeSummary(
            c.Id,
            c.Page?.Id ?? -1,
            c.DataVersion,
            c.Data,
            c.AuthorId,
            (int)c.Type,
            c.DateCreated
        )).ToList();
    }
}
