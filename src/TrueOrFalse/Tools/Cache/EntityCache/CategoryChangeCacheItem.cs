using Newtonsoft.Json.Linq;
using Seedworks.Lib.Persistence;

[Serializable]
public class CategoryChangeCacheItem : IPersistable
{
    public virtual int Id { get; set; }

    private CategoryCacheItem _categoryCacheItem;
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

    public virtual CategoryCacheItem ToHistoricCategory(bool haveVersionData = true)
    {
        return haveVersionData ? GetCategoryChangeData().ToCacheCategory(_categoryCacheItem.Id) : new CategoryCacheItem();
    }

    public static CategoryChangeCacheItem ToCategoryChangeCacheItem(CategoryChange categoryChange)
    {
        var visibility = CategoryVisibility.Owner;

        if (!string.IsNullOrEmpty(categoryChange.Data))
        {
            var jObject = JObject.Parse(categoryChange.Data);
            if (jObject["Visibility"] != null)
            {
                var visibilityValue = jObject["Visibility"].Value<int>();
                visibility = (CategoryVisibility)visibilityValue;
            }
        }

        return new CategoryChangeCacheItem
        {
            Id = categoryChange.Id,
            CategoryId = categoryChange.Category.Id,
            DataVersion = categoryChange.DataVersion,
            Data = categoryChange.Data,
            AuthorId = categoryChange.AuthorId,
            Type = categoryChange.Type,
            DateCreated = categoryChange.DateCreated,
            Visibility = visibility
        };
    }

    public static IEnumerable<CategoryChangeCacheItem> ToCategoryChangeCacheItems(IEnumerable<CategoryChange> allCategoryChanges)
    {
        return allCategoryChanges.Where(c => c.Category != null).Select(ToCategoryChangeCacheItem);
    }

}