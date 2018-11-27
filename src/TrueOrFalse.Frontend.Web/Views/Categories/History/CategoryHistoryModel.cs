using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public string CategoryUrl;
    public IList<ChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges)
    {
        CategoryName = category.Name;
        CategoryId = category.Id;
        CategoryUrl = Links.CategoryDetail(CategoryName, CategoryId);

        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new ChangeDayModel(
                group.Key, 
                (IList<CategoryChange>) group.OrderByDescending(g => g.DateCreated).ToList()
            ))
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
            ElapsedTime = TimeElapsedAsText.Run(cc.DateCreated),
            DateTime = cc.DateCreated.ToString("dd.MM.yyyy HH:mm"),
            Time = cc.DateCreated.ToString("HH:mm"),
            CategoryChangeId = cc.Id,
            CategoryId = cc.Category.Id,
            CategoryName = cc.Category.Name
        }).ToList();
    }
}

public class ChangeDetailModel
{
    public User Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
}
