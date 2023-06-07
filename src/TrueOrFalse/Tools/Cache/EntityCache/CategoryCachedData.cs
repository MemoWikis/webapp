using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CategoryCachedData
{
    private List<int> _childrenIds = new();

    public IReadOnlyList<int> ChildrenIds => _childrenIds;

    public void AddChildId(int childId)
    {
        if (!_childrenIds.Contains(childId))
            _childrenIds.Add(childId);
    }

    public static string GetIconHtml(CategoryCacheItem category)
    {
        var iconHTML = "";
        switch (category.Type)
        {
            case CategoryType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case CategoryType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case CategoryType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case CategoryType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }
        if (category.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
            iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";

        return iconHTML;
    }

    public void AddChildIds(List<int> childrenIds)
    {
        foreach (var childId in childrenIds)
        {
            AddChildId(childId);
        }
    }

    public void RemoveChildId(int childId)
    {
        if (_childrenIds.Contains(childId))
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
}
