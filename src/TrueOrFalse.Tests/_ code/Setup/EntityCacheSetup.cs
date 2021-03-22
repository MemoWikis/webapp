public class EntityCacheSetup
{
    public static void Simple()
    {
        EntityCache.Init();
        EntityCache.AddOrUpdate(new CategoryCacheItem { Name = "Schule", Id = 682 });
        EntityCache.AddOrUpdate(new CategoryCacheItem { Name = "Studium", Id = 687 });
        EntityCache.AddOrUpdate(new CategoryCacheItem { Name = "Zertifikate", Id = 689 });
        EntityCache.AddOrUpdate(new CategoryCacheItem { Name = "Allgemeinwissen", Id = CategoryRepository.AllgemeinwissenId });
    }
}