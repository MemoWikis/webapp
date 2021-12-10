﻿using Newtonsoft.Json;
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
            case CategoryChangeType.Text:
                typ = "Text";
                break;
            case CategoryChangeType.Relations:
                typ = "Beziehungsdaten";
                break;
            case CategoryChangeType.Image:
                typ = "Bild";
                break;
            case CategoryChangeType.Restore:
                typ = "Wiederherstellung";
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
            ElapsedTime = TimeElapsedAsText.Run(change.DateCreated),
            DateTime = change.DateCreated.ToString("dd.MM.yyyy HH:mm"),
            Time = change.DateCreated.ToString("HH:mm"),
            DateCreated = change.DateCreated,
            CategoryChangeId = change.Id,
            CategoryId = change.Category == null ? categoryId : change.Category.Id,
            CategoryName = change.Category == null ? _catName : change.Category.Name,
            Typ = typ,
            Type = change.Type,
            CreatorId = new UserTinyModel(change.Category.Creator).Id,
            Visibility = change.Category.Visibility,
        };
    }
    public void AppendItems(List<CategoryChangeDetailModel> items, CategoryChange change)
    {
        if (change.Category == null || change.Category.IsNotVisibleToCurrentUser)
            return;

        if (_currentCategoryChangeDetailModel != null &&
            change.Author.Id == _currentCategoryChangeDetailModel.Author.Id &&
            change.Category.Visibility == _currentCategoryChangeDetailModel.Visibility &&
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

public class CategoryChangeViewItem
{
    public UserTinyModel Author;
    public DateTime FirstEdit;
    public DateTime LastEdit;
    public CategoryVisibility Visibility;
    public CategoryChangeType Type;
    public List<CategoryChange> CategoryChanges;
}

public class  CategoryChangeDetailModel
{
    public UserTinyModel Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public DateTime DateCreated;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
    public string Typ;
    public CategoryChangeType Type;
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