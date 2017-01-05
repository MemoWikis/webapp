using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Properties;
using TrueOrFalse.Search;

public class QuestionRepo : RepositoryDbBase<Question>
{
    private readonly SearchIndexQuestion _searchIndexQuestion;

    public QuestionRepo(ISession session, SearchIndexQuestion searchIndexQuestion) : base(session)
    {
        _searchIndexQuestion = searchIndexQuestion;
    }

    public new void Update(Question question)
    {
        _searchIndexQuestion.Update(question);
        base.Update(question);
        Flush();
        Sl.Resolve<UpdateQuestionCountForCategory>().Run(question.Categories);
    }

    public override void Create(Question question)
    {
        if (question.Creator == null)
            throw new Exception("no creator");

        base.Create(question);
        Flush();
        Sl.R<UpdateQuestionCountForCategory>().Run(question.Categories);
        if (question.Visibility != QuestionVisibility.Owner)
        {
            UserActivityAdd.CreatedQuestion(question);
            ReputationUpdate.ForUser(question.Creator);
        }
        _searchIndexQuestion.Update(question);
    }

    public override void Delete(Question question)
    {
        _searchIndexQuestion.Delete(question);
        base.Delete(question);
    }

    public IList<Question> GetForCategory(int categoryId, int resultCount, int currentUser)
    {
        return _session.QueryOver<Question>()
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(x => x.DateCreated).Desc
            .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .Take(resultCount)
            .List<Question>();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public IList<Question> GetForReference(int categoryId, int resultCount, int currentUser)
    {
        return _session.QueryOver<Question>()
            .OrderBy(q => q.TotalRelevancePersonalEntries).Desc
            .ThenBy(q => q.DateCreated).Desc
            .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
            .JoinQueryOver<Reference>(q => q.References)
            .Where(q => q.Category.Id == categoryId)
            .Take(resultCount)
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

    public IList<Question> GetByIds(List<int> questionIds)
    {
        return GetByIds(questionIds.ToArray());
    }

    public override IList<Question> GetByIds(params int[] questionIds)
    {
        var tmpResult = base.GetByIds(questionIds);

        var result = new List<Question>();
        for (int i = 0; i < questionIds.Length; i++)
        {
            if (tmpResult.Any(q => q.Id == questionIds[i]))
                result.Add(tmpResult.First(q => q.Id == questionIds[i]));
        }

        return result;
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
        Func<KnowledgeStatus, AbstractCriterion> fnGetCombinedExpression = knowledgeStatus => {
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

    public IEnumerable<Question> GetMostRecent(int amount)
    {
        return _session
            .QueryOver<Question>()
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

    /// <summary>
    /// Return how often a question is part of a future date
    /// </summary>
    public int HowOftenInFutureDate(int questionId)
    {
        var query = "SELECT COUNT(*) FROM " +
                    "(SELECT qis.Set_id, dts.Date_id, d.User_id, d.DateTime FROM questioninset as qis LEFT JOIN question as q ON qis.Question_id = q.Id " +
                    "LEFT JOIN date_to_sets as dts ON dts.Set_id = qis.Set_id " +
                    "LEFT JOIN date as d ON d.Id = dts.Date_id " +
                    "WHERE qis.Question_id = {0} AND d.DateTime > NOW() GROUP BY dts.Date_id) " +
                    "AS c; ";
        return (int)Sl.R<ISession>()
            .CreateSQLQuery(String.Format(query, questionId))
            .UniqueResult<long>();

        //Sl.R<ISession>()
        //    .CreateSQLQuery(
        //        "SELECT COUNT(*) FROM " +
        //        "(SELECT qis.Set_id, dts.Date_id, d.User_id, d.DateTime FROM questioninset as qis LEFT JOIN question as q ON qis.Question_id = q.Id " +
        //        "LEFT JOIN date_to_sets as dts ON dts.Set_id = qis.Set_id " +
        //        "LEFT JOIN date as d ON d.Id = dts.Date_id " +
        //        "WHERE qis.Question_id = :questionId AND d.DateTime > NOW() GROUP BY dts.Date_id) " +
        //        "AS c; ")
        //    .SetParameter("questionId", questionId)
        //    .ExecuteUpdate();
    }

    public int HowManyNewQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .RowCount();
    }
}