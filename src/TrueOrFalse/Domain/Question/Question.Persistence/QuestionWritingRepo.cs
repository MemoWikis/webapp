using System.Text.RegularExpressions;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using ISession = NHibernate.ISession;

public class QuestionWritingRepo(
    UpdateQuestionCountForPage updateQuestionCountForPage,
    JobQueueRepo _jobQueueRepo,
    ReputationUpdate _reputationUpdate,
    UserReadingRepo _userReadingRepo,
    UserActivityRepo _userActivityRepo,
    QuestionChangeRepo _questionChangeRepo,
    ISession _nhibernateSession,
    SessionUser _sessionUser,
    PageChangeRepo pageChangeRepo,
    PageRepository pageRepository) : RepositoryDbBase<Question>(_nhibernateSession)
{
    public void Create(Question question, PageRepository pageRepository)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        Create(question);
        Flush();

        updateQuestionCountForPage.Run(question.Pages);

        foreach (var page in question.Pages.ToList())
        {
            page.UpdateCountQuestionsAggregated(question.Creator.Id);
            pageRepository.Update(page);
            KnowledgeSummaryUpdate.ScheduleForPage(page.Id, _jobQueueRepo);
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
        var parentPages = pageRepository.GetByIds(parentIds);

        updateQuestionCountForPage.Run(parentPages, userId);
        var safeText = Regex.Replace(question.Text, "<.*?>", "");

        var changeId = DeleteAndGetChangeId(question, userId);

        foreach (var parent in parentPages)
            pageChangeRepo.AddDeletedQuestionEntry(parent, userId, changeId, safeText, question.Visibility);

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
        var pageIds = _nhibernateSession
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" +
                            question.Id)
            .List<int>();
        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Category_id is not null";
        var pageReferences = _nhibernateSession
            .CreateSQLQuery(query)
            .List<int>();
        var pagesBeforeUpdateIds = pageIds.Union(pageReferences)
            .ToList();

        var pagesBeforeUpdate = _nhibernateSession
            .QueryOver<Page>()
            .WhereRestrictionOn(c => c.Id)
            .IsIn(pagesBeforeUpdateIds)
            .List();

        if (withMerge)
            Merge(question);
        else
            Update(question);

        Flush();
        var pagesToUpdate = pagesBeforeUpdate
            .Union(question.Pages)
            .Union(question.References.Where(r => r.Page != null)
                .Select(r => r.Page))
            .ToList();

        var pagesToUpdateIds = pagesToUpdate.Select(c => c.Id).ToList();

        UpdateQuestionCacheItem(question, pagesToUpdateIds);

        updateQuestionCountForPage.Run(pagesToUpdate);
        JobScheduler.StartImmediately_UpdateAggregatedPagesForQuestion(pagesToUpdateIds,
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
        questionCacheItem.Pages = EntityCache.GetPages(question.Pages?.Select(c => c.Id)).ToList();
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
        EntityCache.AddOrUpdate(questionCacheItem, affectedCategoryIds: categoriesToUpdateIds);
    }
}