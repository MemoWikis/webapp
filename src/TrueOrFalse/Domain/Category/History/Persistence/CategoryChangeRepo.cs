﻿using NHibernate;
using System.Collections.Generic;

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

    public void AddCreateEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(Category category, User author, bool imageWasUpdated) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Update, imageWasUpdated);
    public void AddPublishEntry(Category category, User author, bool imageWasUpdated) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Published, imageWasUpdated);
    public void AddMadePrivateEntry(Category category, User author, bool imageWasUpdated) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Privatized, imageWasUpdated);

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

        var query = _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId);

        if (filterUsersForSidebar)
            query.And(c => c.ShowInSidebar);

        query
            .Left.JoinAlias(c => c.Author, () => aliasUser)
            .Left.JoinAlias(c => c.Category, () => aliasCategory);

        return query
            .List();
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
}