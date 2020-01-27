

using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tools.Update.SetMigration
{
    public class SetMigrator
    {
        public static void Start()
        {
            var allSets = Sl.SetRepo.GetAll();
            var categoryRepo = Sl.CategoryRepo;

            foreach (var set in allSets)
            {
                var category = new Category()
                {
                    Name = set.Name,
                    Type = CategoryType.Standard,
                    Creator = set.Creator,
                    DateCreated = set.DateCreated,
                };

                categoryRepo.Create(category);

                AddParentCategories(category, set.Categories, set.QuestionsInSet);
                MigrateSetValuation(category, set.Id);
                MigrateSetViews(category, set.Id);
            }
        }

        private static void AddParentCategories(Category category, IList<Category> categories, ISet<QuestionInSet> questionInSet)
        {
            foreach (var relatedCategory in categories)
                ModifyRelationsForCategory.AddParentCategory(category, relatedCategory);

            foreach (var question in questionInSet)
                question.Question.Categories.Add(category);
        }

        private static void MigrateSetValuation(Category category, int setId)
        {
            var allSetValuations = Sl.SetValuationRepo.GetAll();
            var setValuationList = allSetValuations.Where(sV => sV.SetId == setId).ToList();

            foreach (var setValuation in setValuationList)
                CreateOrUpdateCategoryValuation.Run(category.Id, setValuation.UserId, setValuation.RelevancePersonal);
        }

        private static void MigrateSetViews(Category category, int setId)
        {
            var categoryViewRepo = Sl.CategoryViewRepo;
            var allSetViews = Sl.SetViewRepo.GetAll();
            var setViewList = allSetViews.Where(sV => sV.Id == setId).ToList();

            foreach (var setView in setViewList)
            {
                var newCategoryView = new CategoryView()
                {
                    Category = category,
                    DateCreated = setView.DateCreated,
                    Id = category.Id,
                    User = setView.User,
                    UserAgent = setView.UserAgent,
                };

                categoryViewRepo.Create(newCategoryView);
            }
        }
    }
}
