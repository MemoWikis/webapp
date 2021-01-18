public class EntityCacheSetup
{
    public static void Simple()
    {
        EntityCache.Init();
        EntityCache.AddOrUpdate(new Category { Name = "Schule", Id = 682 });
        EntityCache.AddOrUpdate(new Category { Name = "Studium", Id = 687 });
        EntityCache.AddOrUpdate(new Category { Name = "Zertifikate", Id = 689 });
        EntityCache.AddOrUpdate(new Category { Name = "Allgemeinwissen", Id = CategoryRepository.AllgemeinwissenId });
    }
}