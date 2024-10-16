﻿using NHibernate;
using NHibernate.Criterion;

public class CategoryChangeRepo(ISession _session) : RepositoryDbBase<CategoryChange>(_session)
{
    public void AddDeleteEntry(Category category,
        int userId)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            AuthorId = userId,
            Type = CategoryChangeType.Delete,
            DataVersion = 2,
            Data = ""
        };

        base.Create(categoryChange);
    }

    public void AddCreateEntry(CategoryRepository categoryRepository, Category category, int authorId) =>
        AddUpdateOrCreateEntry(categoryRepository, category, authorId, CategoryChangeType.Create);
    public void AddCreateEntryDbOnly(CategoryRepository categoryRepository, Category category, User author) =>
        AddUpdateOrCreateEntryDbOnly(categoryRepository, category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(CategoryRepository categoryRepository, Category category, int authorId, bool imageWasUpdated) =>
        AddUpdateOrCreateEntry(categoryRepository, category, authorId, CategoryChangeType.Update, imageWasUpdated);
    public void AddUpdateEntry(
        CategoryRepository categoryRepository,
        Category category,
        int authorId,
        bool imageWasUpdated,
        CategoryChangeType type) =>
        AddUpdateOrCreateEntry(categoryRepository, category, authorId, type, imageWasUpdated);

    private void AddUpdateOrCreateEntry(CategoryRepository categoryRepository, Category category, int authorId, CategoryChangeType categoryChangeType, bool imageWasUpdated = false)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            AuthorId = authorId,
            DataVersion = 2
        };
        var categoryCacheItem = EntityCache.GetCategory(category);

        if (category.AuthorIds == null)
        {
            category.AuthorIds = "";
        }
        else if (AuthorWorthyChangeCheck(categoryChangeType) && authorId > 0 && category.AuthorIdsInts.All(id => id != authorId))
        {
            var newAuthorIdsInts = category.AuthorIdsInts.ToList();
            newAuthorIdsInts.Add(authorId);
            category.AuthorIds = string.Join(",", newAuthorIdsInts.Distinct());
            categoryCacheItem.AuthorIds = category.AuthorIdsInts.Distinct().ToArray();
            //the line should not be needed
            EntityCache.AddOrUpdate(categoryCacheItem);
            categoryRepository.Update(category);
        }

        var parentIds = categoryCacheItem.ParentRelations.Any()
            ? GetParentIds(categoryCacheItem.ParentRelations)
            : null;

        var childIds = categoryCacheItem.ChildRelations.Any()
            ? GetChildIds(categoryCacheItem.ChildRelations)
            : null;

        SetData(categoryRepository, category, imageWasUpdated, categoryChange, parentIds: parentIds, childIds: childIds);
        base.Create(categoryChange);
        categoryCacheItem.AddCategoryChangeToCategoryChangeCacheItems(categoryChange);
    }

    private int[] GetParentIds(IList<CategoryCacheRelation> parentRelations)
    {
        var parentIds = new List<int>();

        foreach (var parentRelation in parentRelations)
        {
            parentIds.Add(parentRelation.ParentId);
        }

        return parentIds.ToArray();
    }

    private int[] GetChildIds(IList<CategoryCacheRelation> childrenRelations)
    {
        var childrenIds = new List<int>();

        foreach (var childrenRelation in childrenRelations)
        {
            childrenIds.Add(childrenRelation.ChildId);
        }

        return childrenIds.ToArray();
    }

    private void SetData(CategoryRepository categoryRepository, Category category, bool imageWasUpdated, CategoryChange categoryChange, int[]? affectedParentIds = null, int[]? parentIds = null, int[]? childIds = null)
    {
        switch (categoryChange.DataVersion)
        {
            case 1:
                categoryChange.Data = new CategoryEditData_V1(category, _session, categoryRepository).ToJson();
                break;
            case 2:
                categoryChange.Data = new CategoryEditData_V2(category, imageWasUpdated, affectedParentIds, _session, parentIds, childIds).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {categoryChange.DataVersion} for category change id {categoryChange.Id}");
        }
    }

    public IList<CategoryChange> GetForCategory(int categoryId, bool filterUsersForSidebar = false)
    {
        Category aliasCategory = null;
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var childIds = categoryCacheItem
            .ParentRelations
            .Select(cr => cr.ParentId)
            .ToList();

        var query = _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId || c.Category.Id.IsIn(childIds));

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Category, () => aliasCategory);

        var categoryChangeList = query
            .List();

        categoryChangeList = categoryChangeList.Where(cc =>
            cc.Category.Id == categoryId ||
            cc.Type != CategoryChangeType.Text &&
            cc.Type != CategoryChangeType.Image &&
            cc.Type != CategoryChangeType.Restore &&
            cc.Type != CategoryChangeType.Update &&
            cc.Type != CategoryChangeType.Relations)
            .ToList();

        return categoryChangeList;
    }

    public IList<CategoryChange> GetForTopic(int categoryId, bool filterUsersForSidebar = false)
    {
        Category aliasCategory = null;

        var query = _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId);

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query.Left.JoinAlias(c => c.Category, () => aliasCategory);

        var categoryChangeList = query
            .List();


        categoryChangeList = categoryChangeList.Where(cc =>
                cc.Category.Id == categoryId ||
                cc.Type != CategoryChangeType.Text &&
                cc.Type != CategoryChangeType.Image &&
                cc.Type != CategoryChangeType.Restore &&
                cc.Type != CategoryChangeType.Update &&
                cc.Type != CategoryChangeType.Relations)
            .ToList();

        return categoryChangeList;
    }

    public bool AuthorWorthyChangeCheck(CategoryChangeType type)
    {
        if (type != CategoryChangeType.Privatized &&
            type != CategoryChangeType.Relations &&
            type != CategoryChangeType.Restore &&
            type != CategoryChangeType.Moved)
            return true;

        return false;
    }

    public CategoryChange GetByIdEager(int categoryChangeId)
    {
        return _session
            .QueryOver<CategoryChange>()
            .Where(cc => cc.Id == categoryChangeId)
            .Left.JoinQueryOver(q => q.Category)
            .SingleOrDefault();
    }

    public int GetCategoryId(int version)
    {
        var query = @"Select Category_id FROM categorychange where id = :version";
        return _session.CreateSQLQuery(query)
            .SetParameter("version", version)
            .UniqueResult<int>();
    }
    private void AddUpdateOrCreateEntryDbOnly(CategoryRepository categoryRepository, Category category,
        User author,
        CategoryChangeType categoryChangeType,
        bool imageWasUpdated = false)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            AuthorId = author.Id,
            DataVersion = 2
        };
        if (category.AuthorIds == null)
        {
            category.AuthorIds = "";
        }
        else if (AuthorWorthyChangeCheck(categoryChangeType) && author.Id > 0 && category.AuthorIdsInts.All(id => id != author.Id))
        {
            var newAuthorIdsInts = category.AuthorIdsInts.ToList();
            newAuthorIdsInts.Add(author.Id);
            category.AuthorIds = string.Join(",", newAuthorIdsInts.Distinct());
            categoryRepository.UpdateOnlyDb(category);
        }
        SetData(categoryRepository, category, imageWasUpdated, categoryChange);
        base.Create(categoryChange);
    }
}