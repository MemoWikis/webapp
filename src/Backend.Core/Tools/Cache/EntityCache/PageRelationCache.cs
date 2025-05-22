[Serializable]
public class PageRelationCache : IPersistable
{
    public virtual int Id { get; set; }
    public virtual int ChildId { get; set; }
    public virtual int ParentId { get; set; }
    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }

    
    public PageCacheItem? Child => EntityCache.GetPage(ChildId);
    public PageCacheItem? Parent => EntityCache.GetPage(ParentId);

    public static IEnumerable<PageRelationCache> ToPageRelationCache(IEnumerable<PageRelation> allRelations)
    {
        return allRelations.Select(ToPageCacheRelation);
    }

    public static PageRelationCache ToPageCacheRelation(PageRelation pageRelation)
    {
        return new PageRelationCache
        {
            Id = pageRelation.Id,
            ChildId = pageRelation.Child.Id,
            ParentId = pageRelation.Parent.Id,
            PreviousId = pageRelation.PreviousId,
            NextId = pageRelation.NextId
        };
    }
}