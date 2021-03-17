public class RootCategory
{
    public const int RootCategoryId = 1;
    public static CategoryCacheItem Get => EntityCache.GetCategoryCacheItem(RootCategoryId);
}