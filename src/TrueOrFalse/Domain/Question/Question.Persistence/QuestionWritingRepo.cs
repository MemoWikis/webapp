using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using ISession = NHibernate.ISession;

public class QuestionWritingRepo : RepositoryDbBase<Question>
{
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;
    private readonly JobQueueRepo _jobQueueRepo;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly ISession _nhibernateSession;
    private readonly RepositoryDb<Question> _repo;
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryRepository _categoryRepository;

    public QuestionWritingRepo(
        UpdateQuestionCountForCategory updateQuestionCountForCategory,
        JobQueueRepo jobQueueRepo,
        ReputationUpdate reputationUpdate,
        UserReadingRepo userReadingRepo,
        UserActivityRepo userActivityRepo,
        QuestionChangeRepo questionChangeRepo,
        ISession nhibernateSession,
        SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryRepository categoryRepository) : base(nhibernateSession)
    {
        _repo = new RepositoryDb<Question>(nhibernateSession);
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _jobQueueRepo = jobQueueRepo;
        _reputationUpdate = reputationUpdate;
        _userReadingRepo = userReadingRepo;
        _userActivityRepo = userActivityRepo;
        _questionChangeRepo = questionChangeRepo;
        _nhibernateSession = nhibernateSession;
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryRepository = categoryRepository;
    }

    public void Create(Question question, CategoryRepository categoryRepository)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        _repo.Create(question);
        _repo.Flush();

        _updateQuestionCountForCategory.Run(question.Categories);

        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated(question.Creator.Id);
            categoryRepository.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id, _jobQueueRepo);
        }

        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question, _userReadingRepo, _userActivityRepo);
            _reputationUpdate.ForUser(question.Creator);
        }

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question));

        _questionChangeRepo.AddCreateEntry(question);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .CreateAsync(question));
    }

    public List<int> Delete(int questionId, int userId)
    {
        var question = GetById(questionId);
        var categoriesToUpdate = question.Categories.ToList();
        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        _updateQuestionCountForCategory.Run(categoriesToUpdate);

        Delete(question, userId);
        return categoriesToUpdateIds;
    }

    public void Delete(Question question, int userId)
    {
        base.Delete(question);
        _questionChangeRepo.AddDeleteEntry(question, userId);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .DeleteAsync(question));
    }

    public void UpdateOrMerge(Question question, bool withMerge)
    {
        var categoriesIds = _nhibernateSession
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" +
                            question.Id)
            .List<int>();
        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Category_id is not null";
        var categoriesReferences = _nhibernateSession
            .CreateSQLQuery(query)
            .List<int>();
        var categoriesBeforeUpdateIds = categoriesIds.Union(categoriesReferences)
            .ToList();

        var categoriesBeforeUpdate = _nhibernateSession
            .QueryOver<Category>()
            .WhereRestrictionOn(c => c.Id)
            .IsIn(categoriesBeforeUpdateIds)
            .List();

        //UpdateOrMerge(question, withMerge);
        if (withMerge)
            Merge(question);
        else
            Update(question);

        Flush();
        var categoriesToUpdate = categoriesBeforeUpdate
            .Union(question.Categories)
            .Union(question.References.Where(r => r.Category != null)
                .Select(r => r.Category))
            .ToList();

        var categoriesToUpdateIds = categoriesToUpdate.Select(c => c.Id).ToList();
        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoriesToUpdateIds);
        _updateQuestionCountForCategory.Run(categoriesToUpdate);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds,
            _sessionUser.UserId);
        _questionChangeRepo.AddUpdateEntry(question);

        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .UpdateAsync(question));
    }

    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }

    public Question UpdateQuestion(
        Question question,
        QuestionDataParam questionDataParam,
        string safeText)
    {
        question.TextHtml = questionDataParam.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = questionDataParam.DescriptionHtml;
        question.SolutionType =
            (SolutionType)Enum.Parse(typeof(SolutionType), questionDataParam.SolutionType);

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = questionDataParam.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories = GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)questionDataParam.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataParam.Solution;
            question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        }
        else
            question.Solution = questionDataParam.Solution;

        question.SolutionMetadataJson = questionDataParam.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(questionDataParam.ReferencesJson))
        {
            var references = ReferenceJson.LoadFromJson(questionDataParam.ReferencesJson, question,
                _categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataParam.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem);

        return question;
    }

    [HttpGet]
    public int GetCurrentQuestionCount([FromRoute] int topicId) => EntityCache.GetCategory(topicId)
        .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;

    public readonly record struct QuestionDataParam(
        int[] CategoryIds,
        int QuestionId,
        string TextHtml,
        string DescriptionHtml,
        dynamic Solution,
        string SolutionMetadataJson,
        int Visibility,
        string SolutionType,
        bool AddToWishknowledge,
        int SessionIndex,
        int LicenseId,
        string ReferencesJson,
        bool IsLearningTab,
        LearningSessionConfig SessionConfig
    );
}