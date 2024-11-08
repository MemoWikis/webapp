public abstract class CategoryEditData
{
    public string Name;
    public string Description;
    public string? TopicMardkown;
    public string? Content;
    public string CustomSegments;
    public string WikipediaURL;
    public bool DisableLearningFunctions;
    public PageVisibility Visibility;
    public int[]? ParentIds;
    public int[]? ChildIds;
    public int? DeleteChangeId;
    public string? DeletedName;

    public abstract string ToJson();

    public abstract Page ToCategory(int categoryId);
    public abstract PageCacheItem ToCacheCategory(int categoryId);
}