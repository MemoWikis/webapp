using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session)
    {
    }

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
        AddUpdateOrCreateEntry(categoryRepository,category, authorId, CategoryChangeType.Create);
    public void AddCreateEntryDbOnly(CategoryRepository categoryRepository, Category category, User author) =>
        AddUpdateOrCreateEntryDbOnly(categoryRepository,category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(CategoryRepository categoryRepository, Category category, int authorId, bool imageWasUpdated) =>    
        AddUpdateOrCreateEntry(categoryRepository, category, authorId, CategoryChangeType.Update, imageWasUpdated);
    public void AddUpdateEntry(CategoryRepository categoryRepository, Category category,
        int authorId,
        bool imageWasUpdated,
        CategoryChangeType type,
        int[] affectedParentIdsByMove = null) =>
        AddUpdateOrCreateEntry(categoryRepository, category, authorId, type, imageWasUpdated, affectedParentIdsByMove);

    private void AddUpdateOrCreateEntry(CategoryRepository categoryRepository, Category category, int authorId, CategoryChangeType categoryChangeType, bool imageWasUpdated = false, int[] affectedParentIdsByMove = null)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            AuthorId = authorId,
            DataVersion = 2
        };
        if (category.AuthorIds == null)
        {
            category.AuthorIds = "";
        }
        if (AuthorWorthyChangeCheck(categoryChangeType) && authorId > 0 && !category.AuthorIds.Contains("," + authorId + ","))
        {
            category.AuthorIds += ", " + authorId;
            var categoryCacheItem = EntityCache.GetCategory(category);
            categoryCacheItem.AuthorIds = category.AuthorIdsInts.Distinct().ToArray();
            EntityCache.AddOrUpdate(categoryCacheItem);
            categoryRepository.Update(category);
        }
        SetData(categoryRepository, category, imageWasUpdated, affectedParentIdsByMove, categoryChange);
        base.Create(categoryChange);
    }


    private void SetData(CategoryRepository categoryRepository, Category category, bool imageWasUpdated, int[] affectedParentIds, CategoryChange categoryChange)
    {
        switch (categoryChange.DataVersion)
        {
            case 1:
                categoryChange.Data = new CategoryEditData_V1(category, _session, categoryRepository).ToJson();
                break;

            case 2:
                categoryChange.Data = new CategoryEditData_V2(category, imageWasUpdated, affectedParentIds, _session).ToJson();
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {categoryChange.DataVersion} for category change id {categoryChange.Id}");
        }
    }

    private void AddUpdateOrCreateEntryDbOnly(CategoryRepository categoryRepository, Category category,
        User author,
        CategoryChangeType categoryChangeType,
        bool imageWasUpdated = false,
        int[] affectedParentIdsByMove = null)
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
        if (AuthorWorthyChangeCheck(categoryChangeType) && author.Id > 0 && !category.AuthorIds.Contains("," + author.Id + ","))
        {
            category.AuthorIds += ", " + author.Id;
            categoryRepository.Update(category);
        }
        SetData(categoryRepository, category, imageWasUpdated, affectedParentIdsByMove, categoryChange);
        base.Create(categoryChange);
    }

    public IList<CategoryChange> GetForCategory(int categoryId, bool filterUsersForSidebar = false)
    {
        Category aliasCategory = null;
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var childIds = categoryCacheItem
            .CategoryRelations
            .Select(cr => cr.RelatedCategoryId)
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
        return Sl.Resolve<ISession>().CreateSQLQuery("Select Category_id FROM categorychange where id = " + version).UniqueResult<int>();
    }
}