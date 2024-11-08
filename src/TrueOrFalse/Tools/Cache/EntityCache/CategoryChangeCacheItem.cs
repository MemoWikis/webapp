using Seedworks.Lib.Persistence;

[Serializable]
public class CategoryChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private PageCacheItem? _categoryCacheItem;
    public virtual PageCacheItem Page => _categoryCacheItem ??= EntityCache.GetPage(CategoryId);
    public virtual int CategoryId { get; set; }

    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    private UserCacheItem? _author;
    public virtual UserCacheItem? Author() => _author ??= EntityCache.GetUserById(AuthorId);
    public virtual int AuthorId { get; set; }

    public virtual PageChangeType Type { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual PageVisibility Visibility { get; set; }

    public virtual CategoryChangeData CategoryChangeData { get; set; }

    public virtual List<CategoryChangeCacheItem> GroupedCategoryChangeCacheItems { get; set; } = new List<CategoryChangeCacheItem>();
    public virtual bool IsGroup => GroupedCategoryChangeCacheItems.Count > 1;
    public virtual bool IsPartOfGroup { get; set; } = false;

    public virtual CategoryEditData GetCategoryChangeData()
    {
        switch (DataVersion)
        {
            case 1:
                return CategoryEditData_V1.CreateFromJson(Data);
            case 2:
                return PageEditData_V2.CreateFromJson(Data);

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for category change id {Id}");
        }
    }

    public virtual PageCacheItem ToHistoricCategoryCacheItem(bool haveVersionData = true)
    {
        return haveVersionData ? GetCategoryChangeData().ToCacheCategory(_categoryCacheItem.Id) : new PageCacheItem();
    }

    public static CategoryChangeCacheItem ToCategoryChangeCacheItem(PageChange currentPageChange, CategoryEditData currentData, CategoryEditData? previousData, int? previousId)
    {
        var changeData = GetCategoryChangeData(currentData, previousData, currentPageChange.Type, previousId);

        return new CategoryChangeCacheItem
        {
            Id = currentPageChange.Id,
            CategoryId = currentPageChange.Page.Id,
            DataVersion = currentPageChange.DataVersion,
            Data = currentPageChange.Data,
            AuthorId = currentPageChange.AuthorId,
            Type = currentPageChange.Type,
            DateCreated = currentPageChange.DateCreated,
            Visibility = currentData.Visibility,
            CategoryChangeData = changeData
        };
    }

    public static bool CanBeGrouped(CategoryChangeCacheItem previousCacheItem, CategoryChangeCacheItem currentCacheItem)
    {
        var timeSpan = previousCacheItem.Type switch
        {
            PageChangeType.Renamed => 2,
            PageChangeType.Relations => 5,
            PageChangeType.Create => 1,
            _ => 10
        };

        if (currentCacheItem.Type == PageChangeType.Create && previousCacheItem.Type == PageChangeType.Relations)
            timeSpan = 1;

        var allowedGroupingTypes = new List<PageChangeType>
        {
            PageChangeType.Text,
            PageChangeType.Renamed,
            PageChangeType.Relations,
            PageChangeType.Create
        };

        return allowedGroupingTypes.Contains(previousCacheItem.Type)
               && Math.Abs((previousCacheItem.DateCreated - currentCacheItem.DateCreated).TotalMinutes) <= timeSpan
               && previousCacheItem.AuthorId == currentCacheItem.AuthorId
               && previousCacheItem.Visibility == currentCacheItem.Visibility
        //&& previousCacheItem.Type == currentCacheItem.Type;
        && (previousCacheItem.Type == currentCacheItem.Type || (previousCacheItem.Type == PageChangeType.Create && currentCacheItem.Type == PageChangeType.Relations));
    }

    public static CategoryChangeCacheItem ToGroupedCategoryChangeCacheItem(List<CategoryChangeCacheItem> groupedCacheItems)
    {
        var oldestCategoryChangeItem = groupedCacheItems.First();
        var newestCategoryChangeItem = groupedCacheItems.Last();

        var changeData = GetCategoryChangeData(newestCategoryChangeItem.GetCategoryChangeData(), oldestCategoryChangeItem.GetCategoryChangeData(), newestCategoryChangeItem.Type, oldestCategoryChangeItem.Id);
        var type = oldestCategoryChangeItem.Type == PageChangeType.Create
            ? PageChangeType.Create
            : newestCategoryChangeItem.Type;

        return new CategoryChangeCacheItem
        {
            Id = newestCategoryChangeItem.Id,
            CategoryId = newestCategoryChangeItem.CategoryId,
            DataVersion = newestCategoryChangeItem.DataVersion,
            Data = newestCategoryChangeItem.Data,
            AuthorId = newestCategoryChangeItem.AuthorId,
            Type = type,
            DateCreated = newestCategoryChangeItem.DateCreated,
            Visibility = newestCategoryChangeItem.Visibility,
            CategoryChangeData = changeData,
            GroupedCategoryChangeCacheItems = groupedCacheItems
        };
    }

    public static CategoryChangeData GetCategoryChangeData(CategoryEditData currentData, CategoryEditData? previousData, PageChangeType changeType, int? previousId)
    {
        return new CategoryChangeData(
            NameChange: new NameChange(previousData?.Name, currentData.Name),
            RelationChange: GetRelationChange(previousData?.ParentIds, previousData?.ChildIds, currentData.ParentIds, currentData.ChildIds),
            PreviousId: previousId,
            VisibilityChange: new VisibilityChange(previousData?.Visibility, currentData.Visibility),
            DeleteData: GetDeleteData(currentData, changeType)
        );
    }

    private static DeleteData? GetDeleteData(CategoryEditData currentData, PageChangeType changeType)
    {
        if (changeType == PageChangeType.ChildTopicDeleted || changeType == PageChangeType.QuestionDeleted)
            return new DeleteData(currentData.DeleteChangeId, currentData.DeletedName);

        return null;
    }

    private static RelationChange GetRelationChange(int[]? previousParentIds, int[]? previousChildIds, int[]? currentParentIds, int[]? currentChildIds)
    {
        var addedParentIds = new List<int>();
        var removedParentIds = new List<int>();
        var addedChildIds = new List<int>();
        var removedChildIds = new List<int>();

        if (previousParentIds != null && currentParentIds != null)
        {
            addedParentIds = currentParentIds.Except(previousParentIds).ToList();
            removedParentIds = previousParentIds.Except(currentParentIds).ToList();
        }
        else if (previousParentIds == null && currentParentIds != null)
        {
            addedParentIds = currentParentIds.ToList();
        }
        else if (previousParentIds != null && currentParentIds == null)
        {
            removedParentIds = previousParentIds.ToList();
        }

        if (previousChildIds != null && currentChildIds != null)
        {
            addedChildIds = currentChildIds.Except(previousChildIds).ToList();
            removedChildIds = previousChildIds.Except(currentChildIds).ToList();
        }
        else if (previousChildIds == null && currentChildIds != null)
        {
            addedChildIds = currentChildIds.ToList();
        }
        else if (previousChildIds != null && currentChildIds == null)
        {
            removedChildIds = previousChildIds.ToList();
        }

        return new RelationChange(addedParentIds, removedParentIds, addedChildIds, removedChildIds);
    }
}

public record struct NameChange(string? OldName, string NewName);
public record struct RelationChange(List<int> AddedParentIds, List<int> RemovedParentIds, List<int> AddedChildIds, List<int> RemovedChildIds);
public record struct VisibilityChange(PageVisibility? OldVisibility, PageVisibility NewVisibility);
public record struct DeleteData(int? DeleteChangeId, string? DeletedName);
public record struct CategoryChangeData(NameChange NameChange, RelationChange RelationChange, int? PreviousId, VisibilityChange VisibilityChange, DeleteData? DeleteData);