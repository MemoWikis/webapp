using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CategoryCachedData
{
    private List<int> _childrenIds = new List<int>();

    public IReadOnlyList<int> ChildrenIds => _childrenIds;

    public void AddChildId(int childId)
    {
        _childrenIds.Add(childId);
    }

    public void AddChildIds(List<int> childrenIds)
    {
        _childrenIds = childrenIds;
    }

    public void RemoveChildId(int childId)
    {
        _childrenIds.Remove(childId);
    }

    public void RemoveChildIds(List<int> childrenIds)
    {
        _childrenIds.RemoveAll(childrenIds.Contains);
    }

    public void ClearChildIds()
    {
        _childrenIds = new List<int>();
    }

    public int GetChild(int id)
    {
        return _childrenIds.First(cId => cId == id);
    }

    public int CountVisibleChildrenIds =>
        EntityCache.GetCategoryCacheItems(ChildrenIds).Where(PermissionCheck.CanView).Distinct().Count();
}
