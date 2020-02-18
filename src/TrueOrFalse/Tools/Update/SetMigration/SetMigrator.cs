using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Razor.Generator;
using Quartz;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.Search;

namespace SetMigration
{
    public class SetMigrator
    {
        private static readonly IList<SetView> allSetViews = Sl.SetViewRepo.GetAll();
        public static void Start()
        {
            Stopwatch migrationTimer = new Stopwatch();
            migrationTimer.Start();
            Logg.r().Information("Migration Start");

            var allSets = Sl.SetRepo.GetAll();
            var categoryRepo = Sl.CategoryRepo;
            var allCategories = Sl.CategoryRepo.GetAll();
            var categories = new List<Category>();

            foreach (var set in allSets)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                Logg.r().Information("Migrating Set: {setId} - Start", set.Id);
                var name = set.Name;
                var duplicateName = false;
                if (allCategories.Any(c => c.Name == set.Name))
                {
                    name = set.Name + " (ehem. Lernset)";
                    duplicateName = true;
                }

                var category = new Category()
                {
                    Name = name,
                    Type = CategoryType.Standard,
                    Creator = set.Creator,
                    DateCreated = set.DateCreated,
                    FormerSetId = set.Id,
                    CountQuestionsAggregated = set.QuestionsPublicCount(),
                };

                categoryRepo.Create(category);
                AddParentCategories(category, set.Categories, set.QuestionsInSet);
                MigrateSetValuation(category, set.Id);
                MigrateSetViews(category, set.Id);
                categoryRepo.Update(category);
                categories.Add(category);
                timer.Stop();
                Logg.r().Information("Migrating Set: {setId} to Category: {categoryId}, elapsed Time: {time} | CategoryName renamed = {duplicatedName}", set.Id, category.Id, timer.Elapsed, duplicateName);
            }

            Sl.Resolve<UpdateQuestionCountForCategory>().Run(categories);
            Sl.CategoryRepo.Flush();
            JobBuilder.Create<RefreshEntityCache>();
            JobBuilder.Create<RecalcKnowledgeSummariesForCategory>();

            migrationTimer.Stop();
            Logg.r().Information("Migration ended, elapsed Time: {time}", migrationTimer.Elapsed);
        }

        private static void AddParentCategories(Category category, IList<Category> categories, ISet<QuestionInSet> questionsInSet)
        {
            var categoriesToUpdate = new List<Category>();
            var categoriesToUpdateDictionary = new Dictionary<int, string>();
            foreach (var questionInSet in questionsInSet)
            {
                var question = questionInSet.Question;
                var questionCategories = question.Categories;

                foreach (var c in questionCategories)
                {
                    if (categoriesToUpdateDictionary.ContainsKey(c.Id))
                        continue;
                    categoriesToUpdateDictionary.Add(c.Id, null);
                    categoriesToUpdate.Add(c);
                }
                questionCategories.Add(category);
                categoriesToUpdate.Add(category);
                Sl.QuestionRepo.UpdateFieldsOnlyForMigration(question);
                EntityCache.AddOrUpdate(question);
            }

            UpdateCountQuestionsAggregatedForSetMigration(categoriesToUpdate);

            foreach (var relatedCategory in categories)
            {
                ModifyRelationsForCategory.AddParentCategory(category, relatedCategory);
                EntityCache.AddOrUpdate(relatedCategory);
            }
        }

        private static void UpdateCountQuestionsAggregatedForSetMigration(List<Category> categoriesToUpdate)
        {
            foreach (var c in categoriesToUpdate)
            {
                c.UpdateCountQuestionsAggregated();
                Sl.CategoryRepo.UpdateWithoutFlush(c);
            }

            Sl.CategoryRepo.Flush();
            Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToUpdate);
            foreach (var c in categoriesToUpdate)
                EntityCache.AddOrUpdate(c);
        }

        private static void MigrateSetValuation(Category category, int setId)
        {
            var setValuations = Sl.SetValuationRepo.GetBy(setId);

            foreach (var setValuation in setValuations)
                CreateOrUpdateCategoryValuation.Run(category.Id, setValuation.UserId, setValuation.RelevancePersonal);
        }

        private static void MigrateSetViews(Category category, int setId)
        {
            var categoryViewRepo = Sl.CategoryViewRepo;
            var setViews = allSetViews.Where(sV => sV.Id == setId).ToList();

            foreach (var setView in setViews)
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
