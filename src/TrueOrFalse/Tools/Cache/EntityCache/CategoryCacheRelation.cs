using Seedworks.Lib.Persistence;

[Serializable]
public class CategoryCacheRelation : IPersistable
{
    public virtual int Id { get; set; }
    public virtual int ChildId { get; set; }
    public virtual int ParentId { get; set; }
    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }

    public IList<CategoryCacheRelation> ToParentRelations(IList<CategoryRelation> parentRelations)
    {
        var result = new List<CategoryCacheRelation>();

        if (parentRelations == null)
            Logg.r.Error("CategoryRelations cannot be null");

        if (parentRelations.Count <= 0 || parentRelations == null)
        {
            return result;
        }
        foreach (var categoryRelation in parentRelations)
        {
            result.Add(ToCategoryCacheRelation(categoryRelation));
        }

        return result;
    }

    public IList<CategoryCacheRelation> ToChildRelations(IList<CategoryRelation> childRelations)
    {
        var sortedList = new List<CategoryCacheRelation>();

        if (childRelations == null)
            Logg.r.Error("CategoryRelations cannot be null");

        if (childRelations.Count <= 0 || childRelations == null)
        {
            return sortedList;
        }

        var current = childRelations.FirstOrDefault(x => x.PreviousId == null);

        while (current != null)
        {
            sortedList.Add(ToCategoryCacheRelation(current));
            current = childRelations.FirstOrDefault(x => x.Child.Id == current.NextId);
        }

        return sortedList;
    }

    public IList<CategoryCacheRelation> Sort(IList<CategoryCacheRelation> childRelations)
    {
        if (childRelations == null)
            Logg.r.Error("CategoryRelations cannot be null");

        if (childRelations.Count <= 0 || childRelations == null)
        {
            return childRelations;
        }

        var current = childRelations.FirstOrDefault(x => x.PreviousId == null);

        var sortedList = new List<CategoryCacheRelation>();

        while (current != null)
        {
            sortedList.Add(current);
            current = childRelations.FirstOrDefault(x => x.ChildId == current.NextId);
        }

        return sortedList;
    }

    public static IEnumerable<CategoryCacheRelation> ToCategoryCacheRelations(IEnumerable<CategoryRelation> allRelations)
    {
        return allRelations.Select(ToCategoryCacheRelation);
    }

    public static CategoryCacheRelation ToCategoryCacheRelation(CategoryRelation categoryRelation)
    {
        return new CategoryCacheRelation
        {
            Id = categoryRelation.Id,
            ChildId = categoryRelation.Child.Id,
            ParentId = categoryRelation.Parent.Id,
            PreviousId = categoryRelation.PreviousId,
            NextId = categoryRelation.NextId
        };
    }

    public static bool IsCategoryRelationEqual(CategoryCacheRelation relation1, CategoryCacheRelation relation2)
    {
        return relation1.ParentId == relation2.ParentId &&
               relation1.ChildId == relation2.ChildId;
    }
}