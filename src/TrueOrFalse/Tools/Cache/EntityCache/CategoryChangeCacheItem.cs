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

    public virtual CategoryChangeRecord CategoryChangeRecord { get; set; }

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

    public static CategoryChangeCacheItem ToCategoryChangeCacheItem(CategoryChange currentCategoryChange, CategoryEditData currentData, CategoryEditData? previousData)
    {
        var changeData = GetCategoryChangeRecord(currentData, previousData, currentCategoryChange.Type);

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
            CategoryChangeRecord = changeData
        };
    }

    public static CategoryChangeRecord GetCategoryChangeRecord(CategoryEditData currentData, CategoryEditData? previousData, CategoryChangeType changeType)
    {
        return new CategoryChangeRecord(
            NameChange: new NameChange(previousData?.Name, currentData.Name),
            RelationChange: GetRelationChange(previousData?.ParentIds, previousData?.ChildIds, currentData.ParentIds, currentData.ChildIds),
            ContentChange: GetContentChange(currentData, previousData),
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

    public static ContentChange GetContentChange(CategoryEditData currentData, CategoryEditData? previousData)
    {
        string previousContent;
        string currentContent;

        if (previousData?.Content != null)
            previousContent = previousData.Content;
        else if (previousData?.TopicMardkown != null)
            previousContent = previousData.TopicMardkown;
        else
            previousContent = "";

        if (currentData.Content != null)
            currentContent = currentData.Content;
        else if (currentData.TopicMardkown != null)
            currentContent = currentData.TopicMardkown;
        else
            currentContent = "";

        HtmlDiff.HtmlDiff diffHelper = new HtmlDiff.HtmlDiff(previousContent, currentContent);
        string diffOutput = diffHelper.Build();

        return new ContentChange(previousContent, currentContent, diffOutput);
    }
}

public record struct NameChange(string? OldName, string NewName);
public record struct RelationChange(List<int> AddedParentIds, List<int> RemovedParentIds, List<int> AddedChildIds, List<int> RemovedChildIds);
public record struct ContentChange(string OldContent, string NewContent, string DiffContent);
public record struct VisibilityChange(CategoryVisibility? OldVisibility, CategoryVisibility NewVisibility);
public record struct CategoryChangeRecord(NameChange NameChange, RelationChange RelationChange, ContentChange ContentChange, VisibilityChange VisibilityChange);