﻿using Seedworks.Lib.Persistence;

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

    public IList<PageRelationCache> ToChildRelations(IList<PageRelation> childRelations)
    {
        var sortedList = new List<PageRelationCache>();

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

    public static IEnumerable<PageRelationCache> ToCategoryCacheRelations(IEnumerable<PageRelation> allRelations)
    {
        return allRelations.Select(ToCategoryCacheRelation);
    }

    public static PageRelationCache ToCategoryCacheRelation(PageRelation pageRelation)
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

    public static bool IsCategoryRelationEqual(PageRelationCache relation1, PageRelationCache relation2)
    {
        return relation1.ParentId == relation2.ParentId &&
               relation1.ChildId == relation2.ChildId;
    }
}