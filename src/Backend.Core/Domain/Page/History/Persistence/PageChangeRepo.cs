using NHibernate;
using NHibernate.Criterion;

public class PageChangeRepo(ISession _session) : RepositoryDbBase<PageChange>(_session)
{
    public int AddDeleteEntry(Page page, int userId)
    {
        var pageChange = new PageChange
        {
            Page = page,
            AuthorId = userId,
            Type = PageChangeType.Delete,
            DataVersion = 2,
            Data = ""
        };

        base.Create(pageChange);
        return pageChange.Id;
    }

    public void AddCreateEntry(PageRepository pageRepository, Page page, int authorId) =>
        AddUpdateOrCreateEntry(pageRepository, page, authorId, PageChangeType.Create);

    public void AddUpdateEntry(
        PageRepository pageRepository,
        Page page,
        int authorId,
        bool imageWasUpdated,
        PageChangeType type) =>
        AddUpdateOrCreateEntry(pageRepository, page, authorId, type, imageWasUpdated);

    private void AddUpdateOrCreateEntry(PageRepository pageRepository, Page page, int authorId, PageChangeType pageChangeType, bool imageWasUpdated = false)
    {
        var pageCacheItem = EntityCache.GetPage(page);
        
        page.AuthorIds ??= "";
        
        if (AuthorWorthyChangeCheck(pageChangeType) && authorId > 0 && page.AuthorIdsInts.All(id => id != authorId))
        {
            var newAuthorIds = page.AuthorIdsInts.ToList();
            newAuthorIds.Add(authorId);
            page.AuthorIds = string.Join(",", newAuthorIds.Distinct());
            pageCacheItem.AuthorIds = page.AuthorIdsInts.Distinct().ToArray();
            EntityCache.AddOrUpdate(pageCacheItem);
        }

        var pageChange = new PageChange
        {
            Page = page,
            Type = pageChangeType,
            AuthorId = authorId,
            DataVersion = 2
        };

        var parentIds = pageCacheItem.ParentRelations.Any()
            ? GetParentIds(pageCacheItem.ParentRelations)
            : null;

        var childIds = pageCacheItem.ChildRelations.Any()
            ? GetChildIds(pageCacheItem.ChildRelations)
            : null;

        SetData(pageRepository, page, imageWasUpdated, pageChange, parentIds: parentIds, childIds: childIds);
        base.Create(pageChange);
        pageCacheItem.AddPageChangeToPageChangeCacheItems(pageChange);
    }

    public void AddDeletedChildPageEntry(Page page, int authorId, int deleteChangeId, string deletedPageName, PageVisibility deletedVisibility)
    {
        var pageChange = new PageChange
        {
            Page = page,
            Type = PageChangeType.ChildPageDeleted,
            AuthorId = authorId,
            DataVersion = 2,
        };

        AddDeleteEntry(pageChange, page, authorId, deleteChangeId, deletedPageName, deletedVisibility);
    }

    public void AddDeletedQuestionEntry(Page page, int authorId, int deleteChangeId, string deletedPageName, QuestionVisibility deletedVisibility)
    {
        var pageChange = new PageChange
        {
            Page = page,
            Type = PageChangeType.QuestionDeleted,
            AuthorId = authorId,
            DataVersion = 2,
        };

        var visibility = (PageVisibility)deletedVisibility;

        AddDeleteEntry(pageChange, page, authorId, deleteChangeId, deletedPageName, visibility);
    }

    private void AddDeleteEntry(PageChange pageChange, Page page, int authorId, int deleteChangeId, string deletedPageName, PageVisibility deletedVisibility)
    {
        var pageCacheItem = EntityCache.GetPage(page);

        page.AuthorIds ??= "";

        var parentIds = pageCacheItem.ParentRelations.Any()
            ? GetParentIds(pageCacheItem.ParentRelations)
            : null;

        var childIds = pageCacheItem.ChildRelations.Any()
            ? GetChildIds(pageCacheItem.ChildRelations)
            : null;

        SetDeleteData(page, pageChange, deleteChangeId, deletedPageName, deletedVisibility, parentIds: parentIds, childIds: childIds);
        base.Create(pageChange);
        pageCacheItem.AddPageChangeToPageChangeCacheItems(pageChange);
    }

    private int[] GetParentIds(IList<PageRelationCache> parentRelations)
    {
        var parentIds = new List<int>();

        foreach (var parentRelation in parentRelations)
        {
            parentIds.Add(parentRelation.ParentId);
        }

        return parentIds.ToArray();
    }

    private int[] GetChildIds(IList<PageRelationCache> childrenRelations)
    {
        var childrenIds = new List<int>();

        foreach (var childrenRelation in childrenRelations)
        {
            childrenIds.Add(childrenRelation.ChildId);
        }

        return childrenIds.ToArray();
    }

    private void SetData(PageRepository pageRepository, Page page, bool imageWasUpdated, PageChange pageChange, int[]? parentIds = null, int[]? childIds = null)
    {
        switch (pageChange.DataVersion)
        {
            case 1:
                pageChange.Data = new PageEditDataV1(page, _session, pageRepository).ToJson();
                break;
            case 2:
                pageChange.Data = new PageEditData_V2(page, imageWasUpdated, _session, parentIds, childIds).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {pageChange.DataVersion} for page change id {pageChange.Id}");
        }
    }

    private void SetDeleteData(Page page, PageChange pageChange, int deleteChangeId, string deletedPageName, PageVisibility deletedVisibility, int[]? parentIds = null, int[]? childIds = null)
    {
        pageChange.Data = new PageEditData_V2(page, imageWasUpdated: false, _session, parentIds, childIds, deleteChangeId: deleteChangeId, deletedName: deletedPageName, deletedVisibility: deletedVisibility).ToJson();
    }

    public IList<PageChange> GetForPage(int pageId, bool filterUsersForSidebar = false)
    {
        Page aliasPage = null;
        var pageCacheItem = EntityCache.GetPage(pageId);
        var childIds = pageCacheItem
            .ParentRelations
            .Select(cr => cr.ParentId)
            .ToList();

        var query = _session
            .QueryOver<PageChange>()
            .Where(c => c.Page.Id == pageId || c.Page.Id.IsIn(childIds));

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Page, () => aliasPage);

        var pageChangeList = query
            .List();

        pageChangeList = pageChangeList
            .Where(pc =>
                pc.Page.Id == pageId ||
                pc.Type != PageChangeType.Text &&
                pc.Type != PageChangeType.Image &&
                pc.Type != PageChangeType.Restore &&
                pc.Type != PageChangeType.Update &&
                pc.Type != PageChangeType.Relations)
            .ToList();

        return pageChangeList;
    }

    public bool AuthorWorthyChangeCheck(PageChangeType type)
    {
        if (type != PageChangeType.Privatized &&
            type != PageChangeType.Relations &&
            type != PageChangeType.Restore &&
            type != PageChangeType.Moved &&
            type != PageChangeType.ChildPageDeleted)
            return true;

        return false;
    }

    public PageChange GetByIdEager(int pageChangeId)
    {
        return _session
            .QueryOver<PageChange>()
            .Where(cc => cc.Id == pageChangeId)
            .Left.JoinQueryOver(q => q.Page)
            .SingleOrDefault();
    }
}