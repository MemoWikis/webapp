using Newtonsoft.Json;
using NHibernate;

public class CategoryChangeRepo : RepositoryDbBase<CategoryChange>
{
    public CategoryChangeRepo(ISession session) : base(session){}

    public void Create(Category category, User author)
    {
        var categoryChange = new CategoryChange
        {
            Data = JsonConvert.SerializeObject(new CategoryEditData_V1(category)),
            Author = author,
            DataVersion = 1
        };

        base.Create(categoryChange);
    }
}