using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using Quartz;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace SetMigration
{
    public class SetMigrator
    {
        private static readonly IList<SetView> _allSetViews = Sl.SetViewRepo.GetAll();
        private static readonly IList<Set> _allSets = Sl.SetRepo.GetAll();
        private static IList<int> _categoryIdsMarkedForDeletion = new List<int>();

        public static void Start()
        {
            Stopwatch migrationTimer = new Stopwatch();
            migrationTimer.Start();
            Logg.r().Information("SetMigration: Start");

            var categoryRepo = Sl.CategoryRepo;
            var allCategories = Sl.CategoryRepo.GetAll();
            var categories = new List<Category>();

            foreach (var set in _allSets)
            {
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
            var setViews = _allSetViews.Where(sV => sV.Id == setId).ToList();

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

        public void UpdateSetMigration()
        {

            Stopwatch migrationTimer = new Stopwatch();
            migrationTimer.Start();
            Logg.r().Information("SetMigrationUpdate: Start");

            foreach (var set in _allSets)
            {
                if (!System.String.IsNullOrEmpty(set.Text) && set.CopiedFrom == null)
                    MigrateSetText(set);

                if (set.CopiedFrom != null)
                    MigrateSetCopies(set);
            }

            DeleteCategories(_categoryIdsMarkedForDeletion);
            _categoryIdsMarkedForDeletion.Clear();
            var session = Sl.Resolve<ISession>();
            session.Clear();
            Sl.CategoryRepo.Flush();
            Sl.CategoryRepo.ClearAllItemCache();

            migrationTimer.Stop();
            Logg.r().Information("SetMigrationUpdate: Migration ended, elapsed Time: {time}", migrationTimer.Elapsed);
        }

        private void MigrateSetText(Set set)
        {
            var category = Sl.CategoryRepo.GetBySetId(set.Id);
            category.TopicMarkdown = set.Text;
            category.Url = set.VideoUrl;
            Sl.CategoryRepo.UpdateWithoutFlush(category);
            Logg.r().Information("SetMigrationUpdate: Set Text from set {sId} migrated to category {cId}", set.Id, category.Id);
        }

        private void MigrateSetCopies(Set set)
        {
            Logg.r().Information("SetMigrationUpdate: Migrating SetCopy {copiedId}", set.Id);

            var copiedQuestionList = set.QuestionsInSet.Select(q => q.Question);
            var baseQuestionList = set.CopiedFrom.QuestionsInSet.Select(q => q.Question);
            var questionDifferenceInBaseSet = baseQuestionList.Except(copiedQuestionList);
            var questionDifferenceInCopiedSet = copiedQuestionList.Except(baseQuestionList);

            if (!questionDifferenceInBaseSet.Any() && !questionDifferenceInCopiedSet.Any() &&
                set.Text == set.CopiedFrom.Text &&
                set.VideoUrl == set.CopiedFrom.VideoUrl)
            {
                UpdateCategoryValuations(set);
                var categoryToDelete = Sl.CategoryRepo.GetBySetId(set.Id);
                _categoryIdsMarkedForDeletion.Add(categoryToDelete.Id);
                Logg.r().Information("SetMigrationUpdate: copied category marked for deletion and set {setId} gets redirected.", set.Id);
            }
            else
                MigrateSetText(set);
        }

        private void UpdateCategoryValuations(Set set)
        {
            Logg.r().Information("SetMigrationUpdate: Start CategoryValuationUpdate");

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

        private void DeleteCategories(IList<int> categoryIdsMarkedForDeletion)
        {
            var isInstallationAdmin = Sl.R<SessionUser>().IsInstallationAdmin;
            var forSetMigration = isInstallationAdmin;

            foreach (var id in categoryIdsMarkedForDeletion)
            {
                var category = Sl.CategoryRepo.GetById(id);
                Sl.Resolve<CategoryDeleter>().Run(category, forSetMigration);
                Logg.r().Information("SetMigrationUpdate: category {cId} deleted", id);
            }
        }
    }
}
