using System.Text.RegularExpressions;
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
    PageChangeRepo _pageChangeRepo,
    PageRepository _pageRepository,
    KnowledgeSummaryUpdateService _knowledgeSummaryUpdateService,
    UpdateAggregatedPagesService _updateAggregatedPagesService) : RepositoryDbBase<Question>(_nhibernateSession)
{
    public override void Create(Question question)
    {
        if (question.Creator == null)
        {
            throw new Exception("no creator");
        }

        base.Create(question);
        Flush();

        updateQuestionCountForPage.Run(question.Pages);

        foreach (var page in question.Pages.ToList())
        {
            page.UpdateCountQuestionsAggregated(question.Creator.Id);
            _pageRepository.Update(page);
            _knowledgeSummaryUpdateService.SchedulePageUpdate(page.Id);
        }

        if (question.Visibility != QuestionVisibility.Private)
        {
            UserActivityAdd.CreatedQuestion(question, _userReadingRepo, _userActivityRepo);
            _reputationUpdate.ForUser(question.Creator);
        }

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question)); _questionChangeRepo.AddCreateEntry(question);
        new MeilisearchQuestionsIndexer().Create(question);
    }

    public List<int> Delete(int questionId, int userId, List<int> parentIds)
    {
        var question = GetById(questionId);
        var parentPages = _pageRepository.GetByIds(parentIds);

        updateQuestionCountForPage.Run(parentPages, userId);
        var safeText = Regex.Replace(question.Text, "<.*?>", "");

        var changeId = DeleteAndGetChangeId(question, userId);

        foreach (var parent in parentPages)
            _pageChangeRepo.AddDeletedQuestionEntry(parent, userId, changeId, safeText, question.Visibility);

        return parentIds;
    }

    public int DeleteAndGetChangeId(Question question, int userId)
    {
        base.Delete(question);
        var changeId = _questionChangeRepo.AddDeleteEntry(question, userId);
        new MeilisearchQuestionsIndexer().Delete(question);
        return changeId;
    }

    public void UpdateOrMerge(Question question, bool withMerge)
    {
        var pageIds = _nhibernateSession
            .CreateSQLQuery("SELECT Page_id FROM pages_to_questions WHERE Question_id =" +
                            question.Id)
            .List<int>();
        var query = "SELECT Page_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Page_id is not null";
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
        _updateAggregatedPagesService.UpdateAggregatedPages(pagesToUpdateIds, _sessionUser.UserId);

        _questionChangeRepo.AddUpdateEntry(question);

        new MeilisearchQuestionsIndexer().Update(question);
    }

    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }

    private void UpdateQuestionCacheItem(Question question, List<int>? pagesToUpdateIds)
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
        EntityCache.AddOrUpdate(questionCacheItem, affectedPageIds: pagesToUpdateIds);
    }

    public void Update(QuestionCacheItem questionCacheItem)
    {
        var question = GetById(questionCacheItem.Id);
        if (question == null)
        {
            throw new Exception("Question not found");
        }

        question.CorrectnessProbability = questionCacheItem.CorrectnessProbability;
        question.CorrectnessProbabilityAnswerCount = questionCacheItem.CorrectnessProbabilityAnswerCount;
        question.Description = questionCacheItem.Description;
        question.DescriptionHtml = questionCacheItem.DescriptionHtml;
        question.LicenseId = questionCacheItem.LicenseId;
        question.SkipMigration = questionCacheItem.SkipMigration;
        question.Solution = questionCacheItem.Solution;
        question.SolutionMetadataJson = questionCacheItem.SolutionMetadataJson;
        question.SolutionType = questionCacheItem.SolutionType;
        question.Text = questionCacheItem.Text;
        question.TextExtended = questionCacheItem.TextExtended;
        question.TextExtendedHtml = questionCacheItem.TextExtendedHtml;
        question.TextHtml = questionCacheItem.TextHtml;
        question.TotalFalseAnswers = questionCacheItem.TotalFalseAnswers;
        question.TotalQualityAvg = questionCacheItem.TotalQualityAvg;
        question.TotalQualityEntries = questionCacheItem.TotalQualityEntries;
        question.TotalRelevanceForAllAvg = questionCacheItem.TotalRelevanceForAllAvg;
        question.TotalRelevanceForAllEntries = questionCacheItem.TotalRelevanceForAllEntries;
        question.TotalRelevancePersonalAvg = questionCacheItem.TotalRelevancePersonalAvg;
        question.TotalRelevancePersonalEntries = questionCacheItem.TotalRelevancePersonalEntries;
        question.TotalTrueAnswers = questionCacheItem.TotalTrueAnswers;
        question.Visibility = questionCacheItem.Visibility;

        Update(question);
    }
}