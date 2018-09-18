using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public IList<ChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges)
    {
        CategoryName = category.Name;
        CategoryId = category.Id;

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
        Date = date.ToString("dd.MM.yyyy");
        Items = changes.Select(cc => new ChangeDetailModel
        {
            Author = cc.Author,
            AuthorName = cc.Author.Name,
            AuthorImageUrl = new UserImageSettings(cc.Author.Id).GetUrl_85px_square(cc.Author).Url,
            Date = TimeElapsedAsText.Run(cc.DateCreated),
            CategoryChangeId = cc.Id
        }).ToList();
    }
}

public class ChangeDetailModel
{
    public User Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string Date;
    public int CategoryChangeId;
}
