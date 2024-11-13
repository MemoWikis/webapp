using Seedworks.Lib.Persistence;

[Serializable]
public class PageChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private PageCacheItem? _pageCacheItem;
    public virtual PageCacheItem Page => _pageCacheItem ??= EntityCache.GetPage(PageId);
    public virtual int PageId { get; set; }

    public virtual int DataVersion { get; set; }
    public virtual string Data { get; set; }

    private UserCacheItem? _author;
    public virtual UserCacheItem? Author() => _author ??= EntityCache.GetUserById(AuthorId);
    public virtual int AuthorId { get; set; }

    public virtual PageChangeType Type { get; set; }
    public virtual DateTime DateCreated { get; set; }

    public virtual PageVisibility Visibility { get; set; }

    public virtual PageChangeData PageChangeData { get; set; }

    public virtual List<PageChangeCacheItem> GroupedPageChangeCacheItems { get; set; } = new List<PageChangeCacheItem>();
    public virtual bool IsGroup => GroupedPageChangeCacheItems.Count > 1;
    public virtual bool IsPartOfGroup { get; set; } = false;

    public virtual PageEditData GetPageChangeData()
    {
        switch (DataVersion)
        {
            case 1:
                return PageEditDataV1.CreateFromJson(Data);
            case 2:
                return PageEditData_V2.CreateFromJson(Data);

            default:
                throw new ArgumentOutOfRangeException($"Invalid data version number {DataVersion} for page change id {Id}");
        }
    }

    public virtual PageCacheItem ToHistoricPageCacheItem(bool haveVersionData = true)
    {
        return haveVersionData ? GetPageChangeData().ToCachePage(_pageCacheItem.Id) : new PageCacheItem();
    }

    public static PageChangeCacheItem ToPageChangeCacheItem(PageChange currentPageChange, PageEditData currentData, PageEditData? previousData, int? previousId)
    {
        var changeData = GetPageChangeData(currentData, previousData, currentPageChange.Type, previousId);

        return new PageChangeCacheItem
        {
            Id = currentPageChange.Id,
            PageId = currentPageChange.Page.Id,
            DataVersion = currentPageChange.DataVersion,
            Data = currentPageChange.Data,
            AuthorId = currentPageChange.AuthorId,
            Type = currentPageChange.Type,
            DateCreated = currentPageChange.DateCreated,
            Visibility = currentData.Visibility,
            PageChangeData = changeData
        };
    }

    public static bool CanBeGrouped(PageChangeCacheItem previousCacheItem, PageChangeCacheItem currentCacheItem)
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

    public static PageChangeCacheItem ToGroupedPageChangeCacheItem(List<PageChangeCacheItem> groupedCacheItems)
    {
        var oldestCategoryChangeItem = groupedCacheItems.First();
        var newestCategoryChangeItem = groupedCacheItems.Last();

        var changeData = GetPageChangeData(newestCategoryChangeItem.GetPageChangeData(), oldestCategoryChangeItem.GetPageChangeData(), newestCategoryChangeItem.Type, oldestCategoryChangeItem.Id);
        var type = oldestCategoryChangeItem.Type == PageChangeType.Create
            ? PageChangeType.Create
            : newestCategoryChangeItem.Type;

        return new PageChangeCacheItem
        {
            Id = newestCategoryChangeItem.Id,
            PageId = newestCategoryChangeItem.PageId,
            DataVersion = newestCategoryChangeItem.DataVersion,
            Data = newestCategoryChangeItem.Data,
            AuthorId = newestCategoryChangeItem.AuthorId,
            Type = type,
            DateCreated = newestCategoryChangeItem.DateCreated,
            Visibility = newestCategoryChangeItem.Visibility,
            PageChangeData = changeData,
            GroupedPageChangeCacheItems = groupedCacheItems
        };
    }

    public static PageChangeData GetPageChangeData(PageEditData currentData, PageEditData? previousData, PageChangeType changeType, int? previousId)
    {
        return new PageChangeData(
            NameChange: new NameChange(previousData?.Name, currentData.Name),
            RelationChange: GetRelationChange(previousData?.ParentIds, previousData?.ChildIds, currentData.ParentIds, currentData.ChildIds),
            PreviousId: previousId,
            VisibilityChange: new VisibilityChange(previousData?.Visibility, currentData.Visibility),
            DeleteData: GetDeleteData(currentData, changeType)
        );
    }

    private static DeleteData? GetDeleteData(PageEditData currentData, PageChangeType changeType)
    {
        if (changeType == PageChangeType.ChildPageDeleted || changeType == PageChangeType.QuestionDeleted)
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
public record struct PageChangeData(NameChange NameChange, RelationChange RelationChange, int? PreviousId, VisibilityChange VisibilityChange, DeleteData? DeleteData);