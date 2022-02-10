using NHibernate;
using System.Collections.Generic;
using System.Linq;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session){}

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

    public void AddCreateEntry(Category category, User author, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, CategoryChangeType.Create);
    public void AddUpdateEntry(Category category, User author, bool imageWasUpdated, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, CategoryChangeType.Update, imageWasUpdated);
    public void AddUpdateEntry(Category category, User author, bool imageWasUpdated, CategoryChangeType type, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, type, imageWasUpdated);
    public void AddPublishEntry(Category category, User author, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, CategoryChangeType.Published);
    public void AddMadePrivateEntry(Category category, User author, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, CategoryChangeType.Privatized);
    public void AddTitleIsChangedEntry(Category category, User author, List<int> parentCategoryIds = null) => AddUpdateOrCreateEntry(category, parentCategoryIds, author, CategoryChangeType.Renamed);

    private void AddUpdateOrCreateEntry(Category category,List<int> parentCategoryIds, User author, CategoryChangeType categoryChangeType, bool imageWasUpdated = false)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            Author = author,
            DataVersion = 2
        };

        if (parentCategoryIds != null && parentCategoryIds.Count > 0)
        {
            categoryChange.ParentCategoryIds = parentCategoryIds;
        }
        
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
        Category aliasParentCategory = null;


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
        return categoryChangeList;
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
        var parentCategoryIds = categoryChange.ParentCategoryIds.ToString();
        var currentRevisionDate = categoryChange.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated > '{currentRevisionDate}' and cc.Parent_Category_Id = {parentCategoryIds} 
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
        return Sl.Resolve<ISession>().CreateSQLQuery("Select Parent_Category_id FROM categorychange where id = " + version).UniqueResult<int>();
    }
}