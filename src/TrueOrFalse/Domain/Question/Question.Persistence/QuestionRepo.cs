﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using SolrNet.Mapping.Validation.Rules;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionRepo : RepositoryDbBase<Question>
{
    private readonly SearchIndexQuestion _searchIndexQuestion;

    public QuestionRepo(ISession session, SearchIndexQuestion searchIndexQuestion) : base(session)
    {
        _searchIndexQuestion = searchIndexQuestion;
    }

    /// <summary>
    /// Basic update method, to be used to update only the internally used statistics.
    /// No cross referencing or validation with other tables is happening here.
    /// </summary>
    public void UpdateFieldsOnly(Question question)
    {
        base.Update(question);
    }

    public new void Update(Question question)
    {
        UpdateOrMerge(question, merge: false);
    }

    public new void Merge(Question question)
    {
        UpdateOrMerge(question, merge: true);
    }

    private void UpdateOrMerge(Question question, bool merge)
    {
        var categoriesIds = _session
            .CreateSQLQuery("SELECT Category_id FROM categories_to_questions WHERE Question_id =" + question.Id)
            .List<int>();
        var query = "SELECT Category_id FROM reference WHERE Question_id=" + question.Id +
                    " AND Category_id is not null";
        var categoriesReferences = _session
            .CreateSQLQuery(query)
            .List<int>();
        var categoriesBeforeUpdateIds = categoriesIds.Union(categoriesReferences);

        _searchIndexQuestion.Update(question);

        if (merge)
            base.Merge(question);
        else
            base.Update(question);

        Flush();

        var categoriesToUpdateIds = categoriesBeforeUpdateIds
            .Union(question.Categories.Select(c => c.Id))
            .Union(question.References.Where(r => r.Category != null)
            .Select(r => r.Category.Id))
            .Distinct()
            .ToList(); //All categories added or removed have to be updated

        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question), categoriesToUpdateIds);
        Sl.Resolve<UpdateQuestionCountForCategory>().Run(categoriesToUpdateIds);
        JobScheduler.StartImmediately_UpdateAggregatedCategoriesForQuestion(categoriesToUpdateIds);
        Sl.QuestionChangeRepo.AddUpdateEntry(question);
    }


    public void UpdateBeforeEntityCacheInit(Question question, bool withSolr = true)
    {
        if (withSolr)
            _searchIndexQuestion.Update(question);
        base.Update(question);

        Flush();
    }

    public override void Create(Question question)
    {
        if (question.Creator == null)
            throw new Exception("no creator");

        base.Create(question);
        Flush();

        Sl.R<UpdateQuestionCountForCategory>().Run(question.Categories);

        foreach (var category in question.Categories.ToList())
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question);
            ReputationUpdate.ForUser(question.Creator);
        }
        _searchIndexQuestion.Update(question);
        EntityCache.AddOrUpdate(QuestionCacheItem.ToCacheQuestion(question));

        Sl.QuestionChangeRepo.AddCreateEntry(question);
    }

    public override void Delete(Question question)
    {
        _searchIndexQuestion.Delete(question);
        base.Delete(question);
        Sl.QuestionChangeRepo.AddDeleteEntry(question);
    }

    public IList<QuestionCacheItem> GetForCategoryAggregated(int categoryId, int currentUser, int resultCount = -1)
    {
        var category = EntityCache.GetCategory(categoryId);

        return category.GetAggregatedQuestionsFromMemoryCache();
    }

    public IList<Question> GetForCategory(int categoryId, int currentUser, int resultCount = -1) =>
        GetForCategory(new List<int> { categoryId }, currentUser, resultCount);

    public IList<Question> GetForCategory(IEnumerable<int> categoryIds, int currentUser, int resultCount = -1)
    {
        var query = _session.QueryOver<Question>()
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(x => x.DateCreated).Desc
            .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(Restrictions.In("Id", categoryIds.ToArray()));

        if (resultCount > -1)
            query.Take(resultCount);

        return query.List<Question>();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public PagedResult<Question> GetForCategoryAndInWishCount(int categoryId, int userId, int resultCount)
    {
        var query = _session.QueryOver<QuestionValuation>()
            .Where(q =>
                q.RelevancePersonal != -1 &&
                q.User.Id == userId)
            .JoinQueryOver(q => q.Question)
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(x => x.DateCreated).Desc
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId);

        return new PagedResult<Question>
        {
            PageSize = resultCount,
            Total = query.RowCount(),
            Items = query
                .Take(resultCount)
                .List<QuestionValuation>()
                .Select(qv => qv.Question)
                .ToList()
        };
    }

    public QuestionCacheItem GetByIdFromMemoryCache(int questionId) => EntityCache.GetQuestionById(questionId);

    public IList<Question> GetByIds(List<int> questionIds) =>
        GetByIds(questionIds.ToArray());

    public override IList<Question> GetByIds(params int[] questionIds)
    {
        var questions = _session
            .QueryOver<Question>()
            .Fetch(q => q.Categories).Eager
            .Where(Restrictions.In("Id", questionIds))
            .List()
            .Distinct()
            .ToList();

        return questionIds
            .Select(questionId => questions.FirstOrDefault(question => question.Id == questionId))
            .Where(q => q != null)
            .ToList();
    }

    public IList<int> GetByKnowledge(
        int userId,
        bool isKnowledgeSolidFilter,
        bool isKnowledgeShouldConsolidateFilter,
        bool isKnowledgeShouldLearnFilter,
        bool isKnowledgeNoneFilter)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q =>
                q.User.Id == userId &&
                q.RelevancePersonal != -1);

        AbstractCriterion knowledgeExpression = null;
        Func<KnowledgeStatus, AbstractCriterion> fnGetCombinedExpression = knowledgeStatus =>
        {
            var knowledgeExpressionToAdd = Restrictions.Eq("KnowledgeStatus", knowledgeStatus);
            return knowledgeExpression != null
                ? Restrictions.Or(knowledgeExpression, knowledgeExpressionToAdd)
                : knowledgeExpressionToAdd;
        };

        if (isKnowledgeSolidFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.Solid);

        if (isKnowledgeShouldConsolidateFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsConsolidation);

        if (isKnowledgeShouldLearnFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NeedsLearning);

        if (isKnowledgeNoneFilter)
            knowledgeExpression = fnGetCombinedExpression(KnowledgeStatus.NotLearned);

        if (knowledgeExpression != null)
            query.And(knowledgeExpression);

        return query
            .JoinQueryOver(q => q.Question)
            .Select(q => q.Question.Id)
            .List<int>();
    }

    public IEnumerable<Question> GetMostRecent(int amount, QuestionVisibility questionVisibility = QuestionVisibility.All)
    {
        return _session
            .QueryOver<Question>()
            .Where(q => q.Visibility == questionVisibility)
            .OrderBy(q => q.DateCreated).Desc
            .Take(amount)
            .List();
    }

    public IList<Question> GetMostViewed(int amount)
    {
        return _session
            .QueryOver<Question>()
            .OrderBy(q => q.TotalViews).Desc
            .Take(amount)
            .List();
    }

    /// <summary>
    /// Return how often a question is in other peoples WuWi
    /// </summary>
    public int HowOftenInOtherPeoplesWuwi(int userId, int questionId)
    {
        return Sl.R<QuestionValuationRepo>()
            .Query
            .Where(v =>
                v.User.Id != userId &&
                v.Question.Id == questionId &&
                v.RelevancePersonal > -1
            )
            .RowCount();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int GetPercentageQuestionsAnsweredMostlyWrong()
    {
        int moreWrongThanRight = _session
            .Query<Question>()
            .Count(q => q.TotalFalseAnswers + q.TotalTrueAnswers > 0 && q.TotalFalseAnswers >= q.TotalTrueAnswers);
        int moreRightThanWrong = _session
            .Query<Question>()
            .Count(q => q.TotalFalseAnswers < q.TotalTrueAnswers);
        return (100 * moreWrongThanRight / (moreRightThanWrong + moreWrongThanRight));
    }

    public IList<Question> GetAllEager()
    {
        //warmup entity cache
        var users = _session
            .QueryOver<User>()
            .Fetch(SelectMode.Fetch, u => u.MembershipPeriods)
            .List();

        var questions = _session.QueryOver<Question>()
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.Categories)
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.References)
            .Future();


        var result = questions.ToList();

        foreach (var question in result)
        {
            NHibernateUtil.Initialize(question.Creator);
            NHibernateUtil.Initialize(question.References);
        }

        return result;
    }
}