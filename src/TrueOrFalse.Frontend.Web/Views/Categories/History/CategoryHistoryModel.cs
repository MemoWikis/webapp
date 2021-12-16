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
    private CategoryHistoryDetailController _categoryHistoryDetailController = new CategoryHistoryDetailController();
    private readonly IOrderedEnumerable<CategoryChange> _listWithAllVersions;

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
        _listWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
    }

    public RelationChangeItem GetRelationChange(CategoryChangeDetailModel item)
    {
        var selectedRevision = CategoryEditData_V2.CreateFromJson(_listWithAllVersions.FirstOrDefault(c => c.Id == item.CategoryChangeId).Data);
        var previousRevision = CategoryEditData_V2.CreateFromJson(_listWithAllVersions.LastOrDefault(c => c.Id < item.CategoryChangeId).Data);

        var relationChangeItem = new RelationChangeItem();

        if (selectedRevision != null && previousRevision != null)
        {
            var selectedRevisionCategoryRelations = selectedRevision.CategoryRelations;
            var previousRevisionCategoryRelations = previousRevision.CategoryRelations;
            var selectedRevNotPreviousRev = selectedRevisionCategoryRelations.Where(l1 => !previousRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));
            var previousRevNotSelectedRev = previousRevisionCategoryRelations.Where(l1 => !selectedRevisionCategoryRelations.Any(l2 => l1.RelatedCategoryId == l2.RelatedCategoryId));

            var count = selectedRevisionCategoryRelations.Count() - previousRevisionCategoryRelations.Count();
            relationChangeItem.RelationAdded = count >= 1;

            var relationChange = selectedRevNotPreviousRev.Concat(previousRevNotSelectedRev).First();
            var categoryIsVisible = EntityCache.GetCategoryCacheItem(relationChange.CategoryId).IsVisibleToCurrentUser();
            var relatedCategory = EntityCache.GetCategoryCacheItem(relationChange.RelatedCategoryId);
            if (!categoryIsVisible || !relatedCategory.IsVisibleToCurrentUser())
                relationChangeItem.IsVisibleToCurrentUser = false;
            relationChangeItem.RelatedCategory = relatedCategory;
            relationChangeItem.Type = relationChange.RelationType;
        }

        return relationChangeItem;
    }

    public class RelationChangeItem
    {
        public bool RelationAdded = false;
        public bool IsVisibleToCurrentUser = true;
        public CategoryRelationType Type;
        public CategoryCacheItem RelatedCategory;
    }

    public bool IsAuthorOrAdmin(CategoryChangeDetailModel item)
    {
        return Sl.SessionUser.IsInstallationAdmin || Sl.SessionUser.UserId == item.Author.Id;
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
                typ = "Veröffentlicht";
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
                typ = "Verknüpfung";
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