using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Quartz;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace SetMigration
{
    public class SetMigrator
    {
        private static readonly IList<SetView> allSetViews = Sl.SetViewRepo.GetAll();
        private static readonly IList<Set> allSets = Sl.SetRepo.GetAll();
        public static void Start()
        {
            Stopwatch migrationTimer = new Stopwatch();
            migrationTimer.Start();
            Logg.r().Information("SetMigration: Start");

            var categoryRepo = Sl.CategoryRepo;
            var allCategories = Sl.CategoryRepo.GetAll();
            var categories = new List<Category>();

            foreach (var set in allSets)
            {
                if (set.Id < 484)
                    continue;

                Stopwatch timer = new Stopwatch();
                timer.Start();
                Logg.r().Information("SetMigration: Migrating Set: {setId} - Start", set.Id);
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
                categoryRepo.UpdateWithoutFlush(category);
                categories.Add(category);
                timer.Stop();
                Logg.r().Information("SetMigration: Migrating Set: {setId} to Category: {categoryId}, elapsed Time: {time} | CategoryName renamed = {duplicatedName}", set.Id, category.Id, timer.Elapsed, duplicateName);
            }

            Sl.Resolve<UpdateQuestionCountForCategory>().Run(categories);
            Sl.CategoryRepo.Flush();
            JobBuilder.Create<RefreshEntityCache>();
            JobBuilder.Create<RecalcKnowledgeSummariesForCategory>();

            migrationTimer.Stop();
            Logg.r().Information("SetMigration: Migration ended, elapsed Time: {time}", migrationTimer.Elapsed);
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
            foreach (var category in categoriesToUpdate)
            {
                category.UpdateCountQuestionsAggregated();
                Sl.CategoryRepo.UpdateWithoutFlush(category);
            }
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

        public static void UpdateSetMigration()
        {
            Stopwatch migrationTimer = new Stopwatch();
            migrationTimer.Start();
            Logg.r().Information("SetMigrationUpdate: Start");

            foreach (var set in allSets)
            {
                if (!System.String.IsNullOrEmpty(set.Text) && set.CopiedFrom == null)
                    MigrateSetText(set);

                if (set.CopiedFrom != null)
                    MigrateSetCopies(set);
            }

            Sl.CategoryRepo.ClearAllItemCache();
            Sl.CategoryRepo.Flush();

            migrationTimer.Stop();
            Logg.r().Information("SetMigrationUpdate: Migration ended, elapsed Time: {time}", migrationTimer.Elapsed);
        }

        private static void MigrateSetText(Set set)
        {
            var category = Sl.CategoryRepo.GetBySetId(set.Id);
            category.TopicMarkdown = set.Text;
            category.Url = set.VideoUrl;
            Sl.CategoryRepo.UpdateWithoutFlush(category);
            Logg.r().Information("SetMigrationUpdate: Set Text from set {sId} migrated to category {cId}", set.Id, category.Id);
        }

        private static void MigrateSetCopies(Set set)
        {
            Logg.r().Information("SetMigrationUpdate: Migrating SetCopy {copiedId}", set.Id);
            var questionDifferenceInBaseSet = set.CopiedFrom.QuestionsInSet.Except(set.QuestionsInSet).ToList();
            var questionDifferenceInCopiedSet = set.QuestionsInSet.Except(set.CopiedFrom.QuestionsInSet).ToList();

            var categoryDifferenceInBaseSet = set.CopiedFrom.Categories.Except(set.Categories).ToList();
            var categoryDifferenceInCopiedSet = set.Categories.Except(set.CopiedFrom.Categories).ToList();

            if (!questionDifferenceInBaseSet.Any() && !questionDifferenceInCopiedSet.Any() && 
                !categoryDifferenceInBaseSet.Any() && !categoryDifferenceInCopiedSet.Any() &&
                set.Text == set.CopiedFrom.Text &&
                set.VideoUrl == set.CopiedFrom.VideoUrl)
            {
                UpdateCategoryValuations(set);
                DeleteSetCopy(set);
            }
            else
                MigrateSetText(set);
        }

        private static void UpdateCategoryValuations(Set set)
        {
            var baseCategory = Sl.CategoryRepo.GetBySetId(set.CopiedFrom.Id);
            var baseCategoryValuations = Sl.CategoryValuationRepo.GetBy(set.CopiedFrom.Id);

            var copiedCategory = Sl.CategoryRepo.GetBySetId(set.Id);
            var copiedCategoryValuations = Sl.CategoryValuationRepo.GetBy(copiedCategory.Id);

            foreach (var copiedCategoryValuation in copiedCategoryValuations)
            {
                var baseCategoryValuation = baseCategoryValuations.FirstOrDefault(v => v.UserId == copiedCategoryValuation.UserId && v.CategoryId == baseCategory.Id);
                if (baseCategoryValuation != null)
                    if (copiedCategoryValuation.RelevancePersonal == baseCategoryValuation.RelevancePersonal)
                        continue;
                if (baseCategoryValuation == null || baseCategoryValuation.DateModified < copiedCategoryValuation.DateModified)
                {
                    CreateOrUpdateCategoryValuation.Run(baseCategory.Id, copiedCategoryValuation.UserId, copiedCategoryValuation.RelevancePersonal);
                }
            }

            Logg.r().Information("SetMigrationUpdate: Updated BaseCategory {baseCategoryId} with {copiedCategoryId} ", baseCategory.Id,  copiedCategory.Id);

        }

        private static void DeleteSetCopy(Set set)
        {
            var categoryToDelete = Sl.CategoryRepo.GetBySetId(set.Id);
            Sl.CategoryRepo.DeleteWithoutFlush(categoryToDelete);
            Logg.r().Information("SetMigrationUpdate: category {cId} deleted, set {sId} gets redirected", categoryToDelete.Id, set.Id);
        }
    }
}
