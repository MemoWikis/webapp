using System.Text.RegularExpressions;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using ISession = NHibernate.ISession;

public class QuestionWritingRepo(
    UpdateQuestionCountForCategory _updateQuestionCountForCategory,
    JobQueueRepo _jobQueueRepo,
    ReputationUpdate _reputationUpdate,
    UserReadingRepo _userReadingRepo,
    UserActivityRepo _userActivityRepo,
    QuestionChangeRepo _questionChangeRepo,
    ISession _nhibernateSession,
    SessionUser _sessionUser,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryRepository _categoryRepository) : RepositoryDbBase<Question>(_nhibernateSession)
{
    public void Create(Question question, CategoryRepository categoryRepository)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        Create(question);
        Flush();

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

    public List<int> Delete(int questionId, int userId, List<int> parentIds)
    {
        var question = GetById(questionId);
        var parentTopics = _categoryRepository.GetByIds(parentIds);

        _updateQuestionCountForCategory.Run(parentTopics, userId);
        var safeText = Regex.Replace(question.Text, "<.*?>", "");

        var changeId = DeleteAndGetChangeId(question, userId);

        foreach (var parent in parentTopics)
            _categoryChangeRepo.AddDeletedQuestionEntry(parent, userId, changeId, safeText, question.Visibility);

        return parentIds;
    }

    public int DeleteAndGetChangeId(Question question, int userId)
    {
        base.Delete(question);
        var changeId = _questionChangeRepo.AddDeleteEntry(question, userId);
        Task.Run(async () => await new MeiliSearchQuestionsDatabaseOperations()
            .DeleteAsync(question));
        return changeId;
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

        UpdateQuestionCacheItem(question, categoriesToUpdateIds);

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

    private void UpdateQuestionCacheItem(Question question, List<int>? categoriesToUpdateIds)
    {
        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionCacheItem == null)
            return;

        questionCacheItem.Visibility = question.Visibility;
        questionCacheItem.Categories = EntityCache.GetCategories(question.Categories?.Select(c => c.Id)).ToList();
        questionCacheItem.DateCreated = question.DateCreated;
        questionCacheItem.DateModified = question.DateModified;
        questionCacheItem.DescriptionHtml = question.DescriptionHtml;
        questionCacheItem.TextExtended = question.TextExtended;
        questionCacheItem.TextExtendedHtml = question.TextExtendedHtml;
        questionCacheItem.Text = question.Text;
        questionCacheItem.TextHtml = question.TextHtml;
        questionCacheItem.SolutionType = question.SolutionType;
        questionCacheItem.LicenseId = question.LicenseId;
        questionCacheItem.Solution = question.Solution;
        questionCacheItem.SolutionMetadataJson = question.SolutionMetadataJson;
        questionCacheItem.License = question.License;

        questionCacheItem.References = ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        EntityCache.AddOrUpdate(questionCacheItem, categoriesToUpdateIds);
    }
}