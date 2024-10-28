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
    SessionUser _sessionUser) : RepositoryDbBase<Question>(_nhibernateSession)
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
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        var questionInEntityCache = EntityCache.GetQuestion(question.Id);
        if (questionInEntityCache != null && questionInEntityCache.QuestionChangeCacheItems.Count > 0)
            questionCacheItem.QuestionChangeCacheItems = questionInEntityCache.QuestionChangeCacheItems;

        EntityCache.AddOrUpdate(questionCacheItem, categoriesToUpdateIds);
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
}