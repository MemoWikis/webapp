

    class UserEntityCacheCategoryRelations
    {
        public virtual UserEntityCacheCategoryItem Category { get; set; }

        public virtual UserEntityCacheCategoryItem RelatedCategory { get; set; }

        public virtual CategoryRelationType CategoryRelationType { get; set; }

        public UserEntityCacheCategoryRelations toUserEntityCacheRelation(CategoryRelation categoryRelation)
        {
            var userCacheCategory = new UserEntityCacheCategoryItem();

           return new UserEntityCacheCategoryRelations
            {
                Category = userCacheCategory.ToCacheCategoryItem(categoryRelation.Category),
                CategoryRelationType = categoryRelation.CategoryRelationType,
                RelatedCategory = userCacheCategory.ToCacheCategoryItem(categoryRelation.RelatedCategory)
            };
        }
    }

