using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    private readonly IOrderedEnumerable<CategoryChange> _listWithAllVersions;

    public CategoryHistoryModel(CategoryCacheItem category, IList<CategoryChange> categoryChanges, int categoryId )
    {
        Data data = new Data();

        if (category == null)
            data = JsonConvert.DeserializeObject<Data>(categoryChanges.First().Data); 

        CategoryName = category == null ?  data.Name :  category.Name;
        CategoryId = categoryId;
        CategoryUrl = Links.CategoryDetail(CategoryName, CategoryId);
        Category = EntityCache.GetCategoryCacheItem(categoryId);
        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key, 
                (IList<CategoryChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
        _listWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
    }

    public RelationChangeItem GetRelationChange(CategoryChangeDetailModel item)
    {
        if (item.Type != CategoryChangeType.Relations)
            return null;

        return RelationChangeItem.GetRelationChangeItem(item, _listWithAllVersions);
    }
}

public class CategoryChangeDayModel
{
    public string Date;
    public IList<CategoryChangeDetailModel> Items;
    private string _catName;
    public DateTime DateTime;
    private CategoryChangeDetailModel _currentCategoryChangeDetailModel;

    public CategoryChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");
        DateTime = date;
        Items = GetItems(changes);
    }

    private List<CategoryChangeDetailModel> GetItems(IList<CategoryChange> changes)
    {
        var items = new List<CategoryChangeDetailModel>();

        foreach (var change in changes)
            AppendItems(items, change);

        return items;
    }

    public CategoryChangeDetailModel GetCategoryChangeDetailModel(CategoryChange change)
    {
        var label = "";
        var categoryId = change.Category == null ? Sl.CategoryChangeRepo.GetCategoryId(change.Id) : -1;

        switch (change.Type)
        {
            case CategoryChangeType.Create:
                label = "Erstellt";
                break;
            case CategoryChangeType.Update:
                label = "Update";
                break;
            case CategoryChangeType.Delete:
                label = "Gelöscht";
                break;
            case CategoryChangeType.Published:
                label = "Veröffentlicht";
                break;
            case CategoryChangeType.Privatized:
                label = "Auf privat gesetzt";
                break;
            case CategoryChangeType.Renamed:
                label = "Umbenannt";
                break;
            case CategoryChangeType.Text:
                label = "Text";
                break;
            case CategoryChangeType.Relations:
                label = "Verknüpfung";
                break;
            case CategoryChangeType.Image:
                label = "Bild";
                break;
            case CategoryChangeType.Restore:
                label = "Wiederherstellung";
                break;
            default:
                Logg.r().Error("CategoryHistoryModel CategoryChangeType is invalid");
                break;
        }

        var userTinyModel = new UserTinyModel(change.Author);

        return new CategoryChangeDetailModel
        {
            Author = userTinyModel,
            AuthorName = userTinyModel.Name,
            AuthorImageUrl = new UserImageSettings(userTinyModel.Id)
                .GetUrl_85px_square(userTinyModel).Url,
            CategoryImageUrl = new CategoryImageSettings(change.Category.Id).GetUrl_50px().Url,
            ElapsedTime = TimeElapsedAsText.Run(change.DateCreated),
            DateTime = change.DateCreated.ToString("dd.MM.yyyy HH:mm"),
            Time = change.DateCreated.ToString("HH:mm"),
            DateCreated = change.DateCreated,
            CategoryChangeId = change.Id,
            CategoryId = change.Category == null ? categoryId : change.Category.Id,
            CategoryName = change.Category == null ? _catName : change.Category.Name,
            Label = label,
            Type = change.Type,
            CreatorId = new UserTinyModel(change.Category.Creator).Id,
            Visibility = change.GetCategoryChangeData().Visibility,
            CurrentVisibility = change.Category.Visibility
        };
    }
    public void AppendItems(List<CategoryChangeDetailModel> items, CategoryChange change)
    {
        if (change.Category == null || !PermissionCheck.CanView(change.Category) || change.Author == null)
            return;

        if (_currentCategoryChangeDetailModel != null &&
            change.Category.Id == _currentCategoryChangeDetailModel.CategoryId &&
            change.Author.Id == _currentCategoryChangeDetailModel.Author.Id &&
            change.GetCategoryChangeData().Visibility == _currentCategoryChangeDetailModel.Visibility &&
            change.Type == CategoryChangeType.Text &&
            _currentCategoryChangeDetailModel.Type == change.Type &&
            (_currentCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Last().DateCreated -
             change.DateCreated).TotalMinutes < 15)
        {
            _currentCategoryChangeDetailModel.AggregatedCategoryChangeDetailModel.Add(GetCategoryChangeDetailModel(change));
        }
        else
        {
            var newDetailModel = GetCategoryChangeDetailModel(change);
            newDetailModel.AggregatedCategoryChangeDetailModel = new List<CategoryChangeDetailModel> { newDetailModel };
            _currentCategoryChangeDetailModel = newDetailModel;
            items.Add(newDetailModel);
        }
    }
}

public class CategoryChangeDetailModel
{
    public UserTinyModel Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string CategoryImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public DateTime DateCreated;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
    public string Label;
    public CategoryChangeType Type;
    public CategoryVisibility Visibility;
    public CategoryVisibility CurrentVisibility;
    public bool IsPrivate => Visibility == CategoryVisibility.Owner || Visibility == CategoryVisibility.OwnerAndFriends;
    public int CreatorId;
    public bool IsVisibleToCurrentUser()
    {
        return (CurrentVisibility == CategoryVisibility.All && Visibility == CategoryVisibility.All) || Sl.SessionUser.IsLoggedInUser(CreatorId);
    }
    public bool RelationIsVisibleToCurrentUser = true;
    public List<CategoryChangeDetailModel> AggregatedCategoryChangeDetailModel;

    public void SetLabelAndVisibility(RelationChangeItem relationChangeItem)
    {
        if (relationChangeItem == null)
        {
            Type = CategoryChangeType.Update;
            Label = "Update";
            return;
        }

        RelationIsVisibleToCurrentUser = relationChangeItem.IsVisibleToCurrentUser;

        if (relationChangeItem.RelationAdded)
            Label += " hinzugefügt";
        else
            Label += " entfernt";
    }
}

public class Data
{
    public string Name;
    public string Description; 
}