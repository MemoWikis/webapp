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

    public static string GetIconHtml(PageCacheItem page)
    {
        var iconHTML = "";
        switch (page.Type)
        {
            case PageType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case PageType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case PageType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case PageType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case PageType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case PageType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case PageType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case PageType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case PageType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }

        if (page.Type.GetPageTypeGroup() == PageTypeGroup.Education)
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