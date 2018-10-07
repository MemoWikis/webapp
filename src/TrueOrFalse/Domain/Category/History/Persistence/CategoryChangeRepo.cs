using System.Collections.Generic;
using Newtonsoft.Json;
using NHibernate;

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
            DataVersion = 1
        };

        base.Create(categoryChange);
    }

    public void AddCreateEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Create);
    public void AddUpdateEntry(Category category, User author) => AddUpdateOrCreateEntry(category, author, CategoryChangeType.Update);

    private void AddUpdateOrCreateEntry(Category category, User author, CategoryChangeType categoryChangeType)
    {
        var categoryChange = new CategoryChange
        {
            Category = category,
            Type = categoryChangeType,
            Author = author,
            DataVersion = 1
        };
        
        categoryChange.SetData(category);

        base.Create(categoryChange);
    }

    public IList<CategoryChange> GetAllEager()
    {
        return _session
            .QueryOver<CategoryChange>()
            .Left.JoinQueryOver(q => q.Category)
            .List();
    }

    public IList<CategoryChange> GetForCategory(int categoryId)
    {
        return _session
            .QueryOver<CategoryChange>()
            .Where(c => c.Category.Id == categoryId)
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
}