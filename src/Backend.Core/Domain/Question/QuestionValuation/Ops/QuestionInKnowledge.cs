using System.Text;
using ISession = NHibernate.ISession;

public class QuestionInKnowledge(
    SessionUser _sessionUser,
    ISession _nhibernateSession,
    ReputationUpdate _reputationUpdate,
    QuestionReadingRepo _questionReadingRepo,
    QuestionValuationReadingRepo _questionValuationReadingRepo,
    QuestionValuationWritingRepo _questionValuationWritingRepo,
    ProbabilityCalc_Simple1 _probabilityCalcSimple1,
    AnswerRepo _answerRepo,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public void Pin(int questionId, int userId)
    {
        UpdateRelevancePersonal(questionId, userId);
    }

    public void Unpin(int questionId, int userId)
    {
        UpdateRelevancePersonal(questionId, userId, -1);
    }

    public void Create(QuestionValuation questionValuation)
    {
        _questionValuationReadingRepo.CreateOrUpdate(questionValuation);

        var sb = new StringBuilder();

        sb.Append(GenerateQualityQuery(questionValuation.Question.Id));
        sb.Append(GenerateRelevanceAllQuery(questionValuation.Question.Id));

        sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal",
            questionValuation.Question.Id));
        sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal",
            questionValuation.Question.Id));

        _nhibernateSession.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        _nhibernateSession.Flush();
    }

    private void ChangeTotalInOthersWishknowledge(
        bool isIncrement,
        int userId,
        QuestionCacheItem question)
    {
        if (question.Creator == null || question.Creator.Id == userId)
            return;

        var sign = isIncrement ? "+" : "-";

        _nhibernateSession
            .CreateSQLQuery(
                @"Update user Set TotalInOthersWishknowledge = TotalInOthersWishknowledge " + sign +
                " 1 where id = " +
                question.Creator.Id + ";")
            .ExecuteUpdate();
    }

    private void UpdateRelevancePersonal(int questionId, int userId, int relevance = 50)
    {
        var question = EntityCache.GetQuestionById(questionId);
        ChangeTotalInOthersWishknowledge(relevance == 50, userId, question);
        CreateOrUpdateValuation(questionId, userId, relevance);

        SetUserWishCountQuestions(userId);

        _nhibernateSession.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        _nhibernateSession.Flush();

        _reputationUpdate.ForQuestion(questionId);

        if (relevance != -1)
            new ProbabilityUpdate_Valuation(_nhibernateSession,
                    _questionValuationReadingRepo,
                    _probabilityCalcSimple1,
                    _answerRepo,
                    _extendedUserCache)
                .Run(questionId, userId, _questionReadingRepo, _userReadingRepo);
    }

    public void SetUserWishCountQuestions(int userId)
    {
        var query =
            $@"
            UPDATE user 
            SET WishCountQuestions =
                (SELECT count(id)
                FROM QuestionValuation
                WHERE userId = :userId
                AND RelevancePersonal > 0) 
            WHERE Id = :userId";
        _nhibernateSession.CreateSQLQuery(query).SetParameter("userId", userId).ExecuteUpdate();
        query =
            $@"Select WishCountQuestions From user
            WHERE Id = :userId";

        var wishKnowledgeCount = (int)_nhibernateSession.CreateSQLQuery(query)
            .SetParameter("userId", userId).UniqueResult();
        _sessionUser.User.WishCountQuestions = wishKnowledgeCount;
    }

    private static string GenerateQualityQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalQuality", "Quality", questionId) + " " +
            GenerateAvgQuery("TotalQuality", "Quality", questionId);
    }

    private static string GenerateRelevancePersonal(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionId) + " " +
            GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionId);
    }

    public void UpdateTotalRelevancePersonalInCache(IList<QuestionCacheItem> questions)
    {
        var questionValuations = new QuestionValuationCache(_extendedUserCache)
            .GetByQuestionsFromCache(questions);
        foreach (var question in questions)
        {
            var totalRelevancePersonalEntriesCount =
                questionValuations.Count(v => v.Question.Id == question.Id && v.IsInWishknowledge);
            question.TotalRelevancePersonalEntries = totalRelevancePersonalEntriesCount;
        }
    }

    private static string GenerateRelevanceAllQuery(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevanceForAll", "RelevanceForAll", questionId) + " " +
            GenerateAvgQuery("TotalRelevanceForAll", "RelevanceForAll", questionId);
    }

    private static string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Avg = " +
               "ROUND((SELECT SUM(" + fieldSource + ") FROM QuestionValuation " +
               " WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1)/ " +
               fieldToSet + "Entries) " +
               "WHERE Id = " + questionId + ";";
    }

    private static string GenerateEntriesQuery(
        string fieldToSet,
        string fieldSource,
        int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Entries = " +
               "(SELECT COUNT(Id) FROM QuestionValuation " +
               "WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1) " +
               "WHERE Id = " + questionId + ";";
    }

    private void CreateOrUpdateValuation(int questionId, int userId, int relevancePersonal = -2)
    {
        var questionValuation = _questionValuationReadingRepo.GetBy(questionId, userId);
        var question = EntityCache.GetQuestion(questionId);

        if (questionValuation == null)
            CreateOrUpdateValuation(question, userId, relevancePersonal);
        else
            CreateOrUpdateValuation(questionValuation, relevancePersonal);
    }

    private void CreateOrUpdateValuation(
        QuestionCacheItem question,
        int userId,
        int relevancePersonal = -2)
    {
        var newQuestionVal = new QuestionValuation
        {
            Question = _questionReadingRepo.GetById(question.Id),
            User = _userReadingRepo.GetById(userId),
            RelevancePersonal = relevancePersonal,
            CorrectnessProbability = question.CorrectnessProbability
        };

        _questionValuationReadingRepo.Create(newQuestionVal);
        _sessionUser.User.AddOrUpdateQuestionValuations(newQuestionVal.ToCacheItem());

        _questionValuationReadingRepo.Flush();
    }

    private void CreateOrUpdateValuation(
        QuestionValuation questionValuation,
        int relevancePersonal = -2)
    {
        if (relevancePersonal != -2)
            questionValuation.RelevancePersonal = relevancePersonal;

        _questionValuationWritingRepo.Update(questionValuation);
        _sessionUser.User.AddOrUpdateQuestionValuations(questionValuation.ToCacheItem());

        _questionValuationReadingRepo.Flush();
    }
}