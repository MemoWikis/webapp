using Seedworks.Lib.Persistence;

[Serializable]
public class PageRelationCache : IPersistable
{
    public virtual int Id { get; set; }
    public virtual int ChildId { get; set; }
    public virtual int ParentId { get; set; }
    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }

    public IList<PageRelationCache> ToParentRelations(IList<PageRelation> parentRelations)
    {
        var result = new List<PageRelationCache>();

        if (parentRelations == null)
            Logg.r.Error("PageRelations cannot be null");

        if (parentRelations.Count <= 0 || parentRelations == null)
        {
            return result;
        }
        foreach (var parentRelation in parentRelations)
        {
            result.Add(ToPageCacheRelation(parentRelation));
        }

        return result;
    }

    public IList<PageRelationCache> ToChildRelations(IList<PageRelation> childRelations)
    {
        var sortedList = new List<PageRelationCache>();

        if (childRelations == null)
            Logg.r.Error("PageRelations cannot be null");

        if (childRelations.Count <= 0 || childRelations == null)
        {
            return sortedList;
        }

        var current = childRelations.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(ToPageCacheRelation(current));
            current = childRelations.FirstOrDefault(x => x.Child.Id == current.NextId);
        }

        return sortedList;
    }

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