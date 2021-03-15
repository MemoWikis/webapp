

    using System;
    using System.Collections.Generic;
    using NHibernate.Mapping;

    public class UserEntityCacheCategoryRelations
    {
        public virtual UserEntityCacheCategoryItem Category { get; set; }

        public virtual UserEntityCacheCategoryItem RelatedCategory { get; set; }

        public virtual CategoryRelationType CategoryRelationType { get; set; }

        public IList<UserEntityCacheCategoryRelations> ToListCategoryRelations(
            IList<CategoryRelation> listCategoryRelations)
        {
            var result = new List<UserEntityCacheCategoryRelations>();

            if (listCategoryRelations == null)
                Logg.r().Error("CategoryRelations cannot be null" );

            foreach (var listCategoryRelation in listCategoryRelations)
            {
                result.Add(ToUserEntityCacheRelation(listCategoryRelation));
            }

            return result; 
        }

        public UserEntityCacheCategoryRelations ToUserEntityCacheRelation(CategoryRelation categoryRelation)
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

