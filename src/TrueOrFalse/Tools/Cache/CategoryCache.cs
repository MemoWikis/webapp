using System.Web;
using System.Web.Caching;

public class CategoryCache
{
    private const string _cacheKeyCategories = "allCategories_CategoryCache";
    private const string _cacheKeyCategoryRelations = "allCategoryRelations_CategoryCache";

    public static void Init()
    {
        var categories = Sl.CategoryRepo.GetAll();
        var aggregatedCategoryRelations = Sl.CategoryRelationRepo.GetAll();

        IntoForeverCache(_cacheKeyCategories, categories);
        IntoForeverCache(_cacheKeyCategoryRelations, aggregatedCategoryRelations);
    }

    private static void IntoForeverCache(string key, object objectToCache)
    {
        HttpRuntime.Cache.Insert(
            key, 
            objectToCache, 
            null, 
            Cache.NoAbsoluteExpiration,
            Cache.NoSlidingExpiration,
            CacheItemPriority.NotRemovable, 
            null);
    }
}