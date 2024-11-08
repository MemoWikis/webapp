﻿using NHibernate;
using NHibernate.Criterion;

public class PageChangeRepo(ISession _session) : RepositoryDbBase<PageChange>(_session)
{
    public int AddDeleteEntry(Page page,
        int userId)
    {
        var categoryChange = new PageChange
        {
            Page = page,
            AuthorId = userId,
            Type = PageChangeType.Delete,
            DataVersion = 2,
            Data = ""
        };

        base.Create(categoryChange);
        return categoryChange.Id;
    }

    public void AddCreateEntry(PageRepository pageRepository, Page page, int authorId) =>
        AddUpdateOrCreateEntry(pageRepository, page, authorId, PageChangeType.Create);
    public void AddCreateEntryDbOnly(PageRepository pageRepository, Page page, User author) =>
        AddUpdateOrCreateEntryDbOnly(pageRepository, page, author, PageChangeType.Create);
    public void AddUpdateEntry(
        PageRepository pageRepository,
        Page page,
        int authorId,
        bool imageWasUpdated,
        PageChangeType type) =>
        AddUpdateOrCreateEntry(pageRepository, page, authorId, type, imageWasUpdated);

    private void AddUpdateOrCreateEntry(PageRepository pageRepository, Page page, int authorId, PageChangeType pageChangeType, bool imageWasUpdated = false)
    {
        var categoryChange = new PageChange
        {
            Page = page,
            Type = pageChangeType,
            AuthorId = authorId,
            DataVersion = 2
        };
        var categoryCacheItem = EntityCache.GetPage(page);

        if (page.AuthorIds == null)
        {
            page.AuthorIds = "";
        }
        else if (AuthorWorthyChangeCheck(pageChangeType) && authorId > 0 && page.AuthorIdsInts.All(id => id != authorId))
        {
            var newAuthorIds = page.AuthorIdsInts.ToList();
            newAuthorIds.Add(authorId);
            page.AuthorIds = string.Join(",", newAuthorIds.Distinct());
            categoryCacheItem.AuthorIds = page.AuthorIdsInts.Distinct().ToArray();
            //the line should not be needed
            EntityCache.AddOrUpdate(categoryCacheItem);
            pageRepository.Update(page);
        }

        var parentIds = categoryCacheItem.ParentRelations.Any()
            ? GetParentIds(categoryCacheItem.ParentRelations)
            : null;

        var childIds = categoryCacheItem.ChildRelations.Any()
            ? GetChildIds(categoryCacheItem.ChildRelations)
            : null;

        SetData(pageRepository, page, imageWasUpdated, categoryChange, parentIds: parentIds, childIds: childIds);
        base.Create(categoryChange);
        categoryCacheItem.AddCategoryChangeToCategoryChangeCacheItems(categoryChange);
    }

    public void AddDeletedChildTopicEntry(Page page, int authorId, int deleteChangeId, string deletedTopicName, PageVisibility deletedVisibility)
    {
        var categoryChange = new PageChange
        {
            Page = page,
            Type = PageChangeType.ChildTopicDeleted,
            AuthorId = authorId,
            DataVersion = 2,
        };

        AddDeleteEntry(categoryChange, page, authorId, deleteChangeId, deletedTopicName, deletedVisibility);
    }

    public void AddDeletedQuestionEntry(Page page, int authorId, int deleteChangeId, string deletedTopicName, QuestionVisibility deletedVisibility)
    {
        var categoryChange = new PageChange
        {
            Page = page,
            Type = PageChangeType.QuestionDeleted,
            AuthorId = authorId,
            DataVersion = 2,
        };

        var visibility = (PageVisibility)deletedVisibility;

        AddDeleteEntry(categoryChange, page, authorId, deleteChangeId, deletedTopicName, visibility);
    }

    private void AddDeleteEntry(PageChange pageChange, Page page, int authorId, int deleteChangeId, string deletedTopicName, PageVisibility deletedVisibility)
    {
        var categoryCacheItem = EntityCache.GetPage(page);

        page.AuthorIds ??= "";

        var parentIds = categoryCacheItem.ParentRelations.Any()
            ? GetParentIds(categoryCacheItem.ParentRelations)
            : null;

        var childIds = categoryCacheItem.ChildRelations.Any()
            ? GetChildIds(categoryCacheItem.ChildRelations)
            : null;

        SetDeleteData(page, pageChange, deleteChangeId, deletedTopicName, deletedVisibility, parentIds: parentIds, childIds: childIds);
        base.Create(pageChange);
        categoryCacheItem.AddCategoryChangeToCategoryChangeCacheItems(pageChange);
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
                pageChange.Data = new CategoryEditData_V1(page, _session, pageRepository).ToJson();
                break;
            case 2:
                pageChange.Data = new PageEditData_V2(page, imageWasUpdated, _session, parentIds, childIds).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {pageChange.DataVersion} for category change id {pageChange.Id}");
        }
    }

    private void SetDeleteData(Page page, PageChange pageChange, int deleteChangeId, string deletedTopicName, PageVisibility deletedVisibility, int[]? parentIds = null, int[]? childIds = null)
    {
        pageChange.Data = new PageEditData_V2(page, imageWasUpdated: false, _session, parentIds, childIds, deleteChangeId: deleteChangeId, deletedName: deletedTopicName, deletedVisibility: deletedVisibility).ToJson();
    }

    public IList<PageChange> GetForCategory(int categoryId, bool filterUsersForSidebar = false)
    {
        Page aliasPage = null;
        var categoryCacheItem = EntityCache.GetPage(categoryId);
        var childIds = categoryCacheItem
            .ParentRelations
            .Select(cr => cr.ParentId)
            .ToList();

        var query = _session
            .QueryOver<PageChange>()
            .Where(c => c.Page.Id == categoryId || c.Page.Id.IsIn(childIds));

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Page, () => aliasPage);

        var categoryChangeList = query
            .List();

        categoryChangeList = categoryChangeList.Where(cc =>
            cc.Page.Id == categoryId ||
            cc.Type != PageChangeType.Text &&
            cc.Type != PageChangeType.Image &&
            cc.Type != PageChangeType.Restore &&
            cc.Type != PageChangeType.Update &&
            cc.Type != PageChangeType.Relations)
            .ToList();

        return categoryChangeList;
    }

    public IList<PageChange> GetForTopic(int categoryId, bool filterUsersForSidebar = false)
    {
        Page aliasPage = null;

        var query = _session
            .QueryOver<PageChange>()
            .Where(c => c.Page.Id == categoryId);

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query.Left.JoinAlias(c => c.Page, () => aliasPage);

        var categoryChangeList = query
            .List();


        categoryChangeList = categoryChangeList.Where(cc =>
                cc.Page.Id == categoryId ||
                cc.Type != PageChangeType.Text &&
                cc.Type != PageChangeType.Image &&
                cc.Type != PageChangeType.Restore &&
                cc.Type != PageChangeType.Update &&
                cc.Type != PageChangeType.Relations)
            .ToList();

        return categoryChangeList;
    }

    public bool AuthorWorthyChangeCheck(PageChangeType type)
    {
        if (type != PageChangeType.Privatized &&
            type != PageChangeType.Relations &&
            type != PageChangeType.Restore &&
            type != PageChangeType.Moved &&
            type != PageChangeType.ChildTopicDeleted)
            return true;

        return false;
    }

    public PageChange GetByIdEager(int categoryChangeId)
    {
        return _session
            .QueryOver<PageChange>()
            .Where(cc => cc.Id == categoryChangeId)
            .Left.JoinQueryOver(q => q.Page)
            .SingleOrDefault();
    }

    public int GetCategoryId(int version)
    {
        var query = @"Select Category_id FROM categorychange where id = :version";
        return _session.CreateSQLQuery(query)
            .SetParameter("version", version)
            .UniqueResult<int>();
    }
    private void AddUpdateOrCreateEntryDbOnly(PageRepository pageRepository, Page page,
        User author,
        PageChangeType pageChangeType,
        bool imageWasUpdated = false)
    {
        var categoryChange = new PageChange
        {
            Page = page,
            Type = pageChangeType,
            AuthorId = author.Id,
            DataVersion = 2
        };
        if (page.AuthorIds == null)
        {
            page.AuthorIds = "";
        }
        else if (AuthorWorthyChangeCheck(pageChangeType) && author.Id > 0 && page.AuthorIdsInts.All(id => id != author.Id))
        {
            var newAuthorIdsInts = page.AuthorIdsInts.ToList();
            newAuthorIdsInts.Add(author.Id);
            page.AuthorIds = string.Join(",", newAuthorIdsInts.Distinct());
            pageRepository.UpdateOnlyDb(page);
        }
        SetData(pageRepository, page, imageWasUpdated, categoryChange);
        base.Create(categoryChange);
    }
}