using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CategoryHistoryModel : BaseModel
{
    public string CategoryName;
    public IList<ChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges)
    {
        CategoryName = category.Name;

        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .Select(group => new ChangeDayModel(group.Key, (IList<CategoryChange>) group))
            .ToList();
    }
}

public class ChangeDayModel
{
    public string Date;
    public IList<ChangeDetailModel> Items;

    public ChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString();
        Items = changes.Select(cc => new ChangeDetailModel
        {
            AuthorName = cc.Author.Name
        }).ToList();
    }
}

public class ChangeDetailModel
{
    public string Title;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ChangeDetailDay;
}
