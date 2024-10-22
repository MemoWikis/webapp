using Seedworks.Lib.Persistence;

[Serializable]
public class CategoryChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private CategoryCacheItem? _categoryCacheItem;
    public virtual CategoryCacheItem Category => _categoryCacheItem ??= EntityCache.GetCategory(CategoryId);
    public virtual int CategoryId { get; set; }

    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    private UserCacheItem? _author;
    public virtual UserCacheItem? Author() => _author ??= EntityCache.GetUserById(AuthorId);
    public virtual int AuthorId { get; set; }

    public virtual CategoryChangeType Type { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual CategoryVisibility Visibility { get; set; }

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
                return CategoryEditData_V2.CreateFromJson(Data);

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for category change id {Id}");
        }
    }

    public virtual CategoryCacheItem ToHistoricCategoryCacheItem(bool haveVersionData = true)
    {
        return haveVersionData ? GetCategoryChangeData().ToCacheCategory(_categoryCacheItem.Id) : new CategoryCacheItem();
    }

    public static CategoryChangeCacheItem ToCategoryChangeCacheItem(CategoryChange currentCategoryChange, CategoryEditData currentData, CategoryEditData? previousData, int? previousId)
    {
        var changeData = GetCategoryChangeData(currentData, previousData, currentCategoryChange.Type, previousId);

        return new CategoryChangeCacheItem
        {
            Id = currentCategoryChange.Id,
            CategoryId = currentCategoryChange.Category.Id,
            DataVersion = currentCategoryChange.DataVersion,
            Data = currentCategoryChange.Data,
            AuthorId = currentCategoryChange.AuthorId,
            Type = currentCategoryChange.Type,
            DateCreated = currentCategoryChange.DateCreated,
            Visibility = currentData.Visibility,
            CategoryChangeData = changeData
        };
    }

    public static bool CanBeGrouped(CategoryChangeCacheItem previousCacheItem, CategoryChangeCacheItem currentCacheItem)
    {
        var timeSpan = previousCacheItem.Type switch
        {
            CategoryChangeType.Renamed => 2,
            CategoryChangeType.Relations => 5,
            _ => 15
        };

        var allowedGroupingTypes = new List<CategoryChangeType>
        {
            CategoryChangeType.Text,
            CategoryChangeType.Renamed,
            CategoryChangeType.Relations
        };

        return allowedGroupingTypes.Contains(previousCacheItem.Type)
               && previousCacheItem.DateCreated - currentCacheItem.DateCreated <= TimeSpan.FromMinutes(timeSpan)
               && previousCacheItem.AuthorId == currentCacheItem.AuthorId
               && previousCacheItem.Visibility == currentCacheItem.Visibility;
    }

    public static CategoryChangeCacheItem ToGroupedCategoryChangeCacheItem(List<CategoryChangeCacheItem> groupedCacheItems)
    {
        var oldestCategoryChangeItem = groupedCacheItems.First();
        var newestCategoryChangeItem = groupedCacheItems.Last();

        var changeData = GetCategoryChangeData(newestCategoryChangeItem.GetCategoryChangeData(), oldestCategoryChangeItem.GetCategoryChangeData(), newestCategoryChangeItem.Type, oldestCategoryChangeItem.Id);

        return new CategoryChangeCacheItem
        {
            Id = newestCategoryChangeItem.Id,
            CategoryId = newestCategoryChangeItem.CategoryId,
            DataVersion = newestCategoryChangeItem.DataVersion,
            Data = newestCategoryChangeItem.Data,
            AuthorId = newestCategoryChangeItem.AuthorId,
            Type = newestCategoryChangeItem.Type,
            DateCreated = newestCategoryChangeItem.DateCreated,
            Visibility = newestCategoryChangeItem.Visibility,
            CategoryChangeData = changeData,
            GroupedCategoryChangeCacheItems = groupedCacheItems
        };
    }

    public static CategoryChangeData GetCategoryChangeData(CategoryEditData currentData, CategoryEditData? previousData, CategoryChangeType changeType, int? previousId)
    {
        return new CategoryChangeData(
            NameChange: new NameChange(previousData?.Name, currentData.Name),
            RelationChange: GetRelationChange(previousData?.ParentIds, previousData?.ChildIds, currentData.ParentIds, currentData.ChildIds),
            PreviousId: previousId,
            VisibilityChange: new VisibilityChange(previousData?.Visibility, currentData.Visibility)
        );
    }

    public static RelationChange GetRelationChange(int[]? previousParentIds, int[]? previousChildIds, int[]? currentParentIds, int[]? currentChildIds)
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
public record struct VisibilityChange(CategoryVisibility? OldVisibility, CategoryVisibility NewVisibility);
public record struct CategoryChangeData(NameChange NameChange, RelationChange RelationChange, int? PreviousId, VisibilityChange VisibilityChange);