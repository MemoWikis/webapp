using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Visitors;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public string CategoryUrl;
    public IList<CategoryChangeDayModel> Days;
    public CategoryCacheItem Category;
    private CategoryHistoryDetailController _categoryHistoryDetailController = new CategoryHistoryDetailController();

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges, int categoryId )
    {
        Data data = new Data(); ;

        if (category == null)
        {
            data = JsonConvert.DeserializeObject<Data>(categoryChanges.First().Data); 
        }

        CategoryName = category == null ?  data.Name :  category.Name;
        CategoryId = categoryId;
        CategoryUrl = Links.CategoryDetail(CategoryName, CategoryId);
        Category = EntityCache.GetCategoryCacheItem(categoryId);

        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key, 
                (IList<CategoryChange>) group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }

    public bool HasChangesVisibleToCurrentUser(int categoryChangeId)
    {
        var hasChanges = false;
        var categoryHistoryDetailModel = _categoryHistoryDetailController.GetCategoryHistoryDetailModel(CategoryId, categoryChangeId);
        return hasChanges;
    }
}

public class CategoryChangeDayModel
{
    public string Date;
    public IList<CategoryChangeDetailModel> Items;
    private string _catName;
    public DateTime DateTime;

    public CategoryChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");
        DateTime = date;
        Items = changes
            .Where(cc => cc.Category != null && cc.Category.IsVisibleToCurrentUser())
            .Select(GetCategoryChangeDetailModel).ToList();
    }

    public CategoryChangeDetailModel GetCategoryChangeDetailModel(CategoryChange change)
    {
        var typ = "";
        var categoryId = change.Category == null ? Sl.CategoryChangeRepo.GetCategoryId(change.Id) : -1;
        switch (change.Type)
        {
            case CategoryChangeType.Create:
                typ = "Erstellt";
                break;
            case CategoryChangeType.Update:
                typ = "Update";
                break;
            case CategoryChangeType.Delete:
                typ = "Gelöscht";
                break;
            case CategoryChangeType.Published:
                typ = "Publish";
                break;
            case CategoryChangeType.Privatized:
                typ = "Auf privat gesetzt";
                break;
            case CategoryChangeType.Renamed:
                typ = "Umbenannt";
                break;
            default:
                Logg.r().Error("CategoryHistoryModel CategoryChangeType is invalid");
                break;
        }

        return new CategoryChangeDetailModel
        {
            Author = new UserTinyModel(change.Author),
            AuthorName = new UserTinyModel(change.Author).Name,
            AuthorImageUrl = new UserImageSettings(new UserTinyModel(change.Author).Id)
                .GetUrl_85px_square(new UserTinyModel(change.Author)).Url,
            ElapsedTime = TimeElapsedAsText.Run(change.DateCreated),
            DateTime = change.DateCreated.ToString("dd.MM.yyyy HH:mm"),
            Time = change.DateCreated.ToString("HH:mm"),
            CategoryChangeId = change.Id,
            CategoryId = change.Category == null ? categoryId : change.Category.Id,
            CategoryName = change.Category == null ? _catName : change.Category.Name,
            Typ = typ,
            CreatorId = new UserTinyModel(change.Category.Creator).Id,
            Visibility = change.Category.Visibility,
        };
    }
}

public class CategoryChangeViewItem
{
    public UserTinyModel Author;
    public DateTime FirstEdit;
    public DateTime LastEdit;
    public CategoryVisibility Visibility;
    public CategoryChangeType Type;
    public List<CategoryChange> CategoryChanges;
}

public class CategoryChangeDetailModel
{
    public UserTinyModel Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
    public string Typ;
    public CategoryVisibility Visibility;
    public bool IsPrivate => Visibility == CategoryVisibility.Owner || Visibility == CategoryVisibility.OwnerAndFriends;
    public int CreatorId;
    public bool IsVisibleToCurrentUser()
    {
        return Visibility == CategoryVisibility.All || Sl.SessionUser.IsLoggedInUser(CreatorId);
    }

    public List<CategoryChangeDetailModel> AggregatedCategoryChangeDetailModel;
}

public class Data
{
    public string Name;
    public string Description; 
}