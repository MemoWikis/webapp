using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session) { }

    public void AddDeleteEntry(Category category)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Author = Sl.SessionUser.User,
            Type = CategoryChangeType.Delete,
            DataVersion = 2,
            Data = ""
        };

        base.Create(categoryChange);
    }

    public void AddCreateEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(Category category, User author, bool imageWasUpdated) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Update, imageWasUpdated);
    public void AddUpdateEntry(Category category, User author, bool imageWasUpdated, CategoryChangeType type) => AddUpdateOrCreateEntry(category, author, type, imageWasUpdated);
    public void AddPublishEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Published);
    public void AddMadePrivateEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Privatized);
    public void AddTitleIsChangedEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Renamed);

    private void AddUpdateOrCreateEntry(Category category, User author, CategoryChangeType categoryChangeType, bool imageWasUpdated = false)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            Author = author,
            DataVersion = 2
        };


        categoryChange.SetData(category, imageWasUpdated);

        base.Create(categoryChange);
    }

    public IList<CategoryChange> GetAllEager()
    {
        return _session
            .QueryOver<CategoryChange>()
            .Left.JoinQueryOver(q => q.Category)
            .List();
    }

    public IList<CategoryChange> GetForCategory(int categoryId, bool filterUsersForSidebar = false)
    {
        User aliasUser = null;
        Category aliasCategory = null;
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var childIds = categoryCacheItem
            .CategoryRelations
            .Where(cci => cci.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(cr => cr.RelatedCategoryId)
            .ToList();

        var query = _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId || c.Category.Id.IsIn(childIds));

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Author, () => aliasUser)
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
    public IList<UserTinyModel> GetAuthorsOfCategory(int categoryId, bool filterUsersForSidebar = false)
    {
        User aliasUser = null;
        Category aliasCategory = null;
        var query = _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId);

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Author, () => aliasUser)
            .Left.JoinAlias(c => c.Category, () => aliasCategory);

        var categoryChangeList = query
            .List();

        return categoryChangeList.Select(categoryChange => new UserTinyModel(categoryChange.Author)).ToList();
    }

    public CategoryChange GetByIdEager(int categoryChangeId)
    {
        return _session
            .QueryOver<CategoryChange>()
            .Where(cc => cc.Id == categoryChangeId)
            .Left.JoinQueryOver(q => q.Category)
            .SingleOrDefault();
    }

    public virtual CategoryChange GetNextRevision(CategoryChange categoryChange)
    {
        var categoryId = categoryChange.Category.Id;
        var currentRevisionDate = categoryChange.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated > '{currentRevisionDate}' 
            ORDER BY cc.DateCreated 
            LIMIT 1
        ";

        var nextRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();
        return nextRevision;
    }

    public virtual int GetCategoryId(int version)
    {
        return Sl.Resolve<ISession>().CreateSQLQuery("Select Category_id FROM categorychange where id = " + version).UniqueResult<int>();
    }
    public virtual int GetParentCategoryId(int version)
    {
        return Sl.Resolve<ISession>().CreateSQLQuery("Select Parent_Category_Ids FROM categorychange where id = " + version).UniqueResult<int>();
    }
}