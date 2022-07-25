
   public  class CacheUpdater
    {
        public  CacheUpdater(Category category)
        {
            var categoryCacheItemOld = EntityCache.GetCategory(category.Id);

            CategoryRepository.UpdateCachedData(categoryCacheItemOld, CategoryRepository.CreateDeleteUpdate.Update);
            EntityCache.AddOrUpdate(categoryCacheItemOld);
            EntityCache.UpdateCategoryReferencesInQuestions(categoryCacheItemOld);
        }
    }

