using System;
using System.Collections.Generic;
using NHibernate;
using TemplateMigration;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs217
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"INSERT INTO Category (Id,Name,Content,DateCreated,DateModified,Type,Creator_id,SkipMigration)Values(1,'Alle Themen','[{\'TemplateName\':\'TopicNavigation\',\'Load\':\'689\'},{\'TemplateName\':\'TopicNavigation\',\'Load\':\'682\'},{\'TemplateName\':\'TopicNavigation\',\'Load\':\'709\'},{\'TemplateName\':\'TopicNavigation\',\'Load\':\'687\'},{\'TemplateName\':\'TopicNavigation\',\'Load\':\'All\'}]','2020-12-18 09:11:59','2020-12-18 09:11:59',1,445,1)")
                .ExecuteUpdate();

            var categories = Sl.CategoryRepo.GetAllEager();
            var rootCategory = Sl.CategoryRepo.GetById(1);
            var categoryRelationsList = new List<CategoryRelation>();
            var arrayFormerlyRootIds = new [] { 682, 689, 709, 687 };


            foreach (var id in arrayFormerlyRootIds)
            {
                var formerlyRootCategoryRelation = new CategoryRelation
                {
                    CategoryRelationType = CategoryRelationType.IsChildCategoryOf,
                    CategoryId = Sl.CategoryRepo.GetById(id),
                    RelatedCategoryId = rootCategory
                };
                categoryRelationsList.Add(formerlyRootCategoryRelation);
            }

            foreach (var category in categories)
            {
                var cr = new CategoryRelation
                {
                    CategoryId = rootCategory,
                    CategoryRelationType = CategoryRelationType.IncludesContentOf,
                    RelatedCategoryId = category
                };
                categoryRelationsList.Add(cr);
            }
            Sl.CategoryRelationRepo.Create(categoryRelationsList);

        }
    }
}