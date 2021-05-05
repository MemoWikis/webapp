
    class CacheUpdater
    {
        public CacheUpdater(Category category)
        {
            var categoryCacheItemOld = EntityCache.GetCategoryCacheItem(category.Id, getDataFromEntityCache: true);

            CategoryRepository.UpdateCachedData(categoryCacheItemOld, CategoryRepository.CreateDeleteUpdate.Update);
            EntityCache.AddOrUpdate(categoryCacheItemOld);
            EntityCache.UpdateCategoryReferencesInQuestions(categoryCacheItemOld, category);
            UserEntityCache.ChangeCategoryInUserEntityCaches(categoryCacheItemOld);
            ModifyRelationsUserEntityCache.UpdateRelationsIncludetContentOf(categoryCacheItemOld);

            Sl.CategoryRepo.Update(category);
    }

    }

