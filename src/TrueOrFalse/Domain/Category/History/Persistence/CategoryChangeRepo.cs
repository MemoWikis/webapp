using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session) { }

    public void AddDeleteEntry(Category category, int userId)
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

    public void AddCreateEntry(Category category, int authorId) => 
        AddUpdateOrCreateEntry(category, authorId, CategoryChangeType.Create);
    public void AddCreateEntryDbOnly(Category category, User author) => 
        AddUpdateOrCreateEntryDbOnly(category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(Category category, int authorId, bool imageWasUpdated) =>
        AddUpdateOrCreateEntry(category, authorId, CategoryChangeType.Update, imageWasUpdated);
    public void AddUpdateEntry(Category category,
        int authorId,
        bool imageWasUpdated,
        CategoryChangeType type,
        int[] affectedParentIdsByMove = null) =>
        AddUpdateOrCreateEntry(category, authorId, type, imageWasUpdated, affectedParentIdsByMove);

    private void AddUpdateOrCreateEntry(Category category, int authorId, CategoryChangeType categoryChangeType, bool imageWasUpdated = false, int[] affectedParentIdsByMove = null)
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
            Sl.CategoryRepo.Update(category);
        }
        categoryChange.SetData(category, imageWasUpdated, affectedParentIdsByMove);
        base.Create(categoryChange);
    }

    private void AddUpdateOrCreateEntryDbOnly(Category category,
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
            Sl.CategoryRepo.Update(category);
        }
        categoryChange.SetData(category, imageWasUpdated, affectedParentIdsByMove);
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
    public IList<int> GetAuthorsOfCategory(int categoryId)
    {
        var sb = "SELECT Author_id FROM categorychange " +
                 "where category_id = " + categoryId +
                 " and (Type = 0 or Type = 1 or Type = 6) " +
                 "and Author_id > 0 " +
                 "Group By Author_id";

        var authorIds = _session.CreateSQLQuery(sb)
            .List<int>();

        return authorIds;
    }

    public bool AuthorWorthyChangeCheck(CategoryChangeType type)
    {
        if (type != CategoryChangeType.Privatized && type != CategoryChangeType.Relations && type != CategoryChangeType.Restore && type != CategoryChangeType.Moved)
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

    public virtual int GetCategoryId(int version)
    {
        return Sl.Resolve<ISession>().CreateSQLQuery("Select Category_id FROM categorychange where id = " + version).UniqueResult<int>();
    }
}