using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse;

public class QuestionInKnowledge : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly ISession _nhibernateSession;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly QuestionRepo _questionRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public QuestionInKnowledge(SessionUser sessionUser,
        ISession nhibernateSession,
        ReputationUpdate reputationUpdate,
        QuestionRepo questionRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _sessionUser = sessionUser;
        _nhibernateSession = nhibernateSession;
        _reputationUpdate = reputationUpdate;
        _questionRepo = questionRepo;
        _questionValuationRepo = questionValuationRepo;
    }
    public void Pin(int questionId, int userId)
    {
        UpdateRelevancePersonal(questionId, userId);
    }

    public void Pin(IList<QuestionCacheItem> questions, User user)
    {
        UpdateRelevancePersonal(questions, user, 50);
    }

    public void Unpin(int questionId, int userId)
    {
        UpdateRelevancePersonal(questionId, userId, -1);
    }

    public void Create(QuestionValuation questionValuation)
    {
        _questionValuationRepo.CreateOrUpdate(questionValuation);

        var sb = new StringBuilder();

        sb.Append(GenerateQualityQuery(questionValuation.Question.Id));
        sb.Append(GenerateRelevanceAllQuery(questionValuation.Question.Id));

        sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        
        
        _nhibernateSession.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        _nhibernateSession.Flush();
    }

    private void ChangeTotalInOthersWishknowledge(bool isIncrement, int userId, QuestionCacheItem question)
    {
        if (question.Creator == null || question.Creator.Id == userId) 
            return; 
           
        var sign = isIncrement ? "+" : "-" ;
        
                _nhibernateSession
                    .CreateSQLQuery(
                @"Update user Set TotalInOthersWishknowledge = TotalInOthersWishknowledge " + sign + " 1 where id = " +
                question.Creator.Id + ";")
                    .ExecuteUpdate();
    }

    private void UpdateRelevancePersonal(IList<QuestionCacheItem> questions, User user, int relevance = 50)
    {
        var questionValuations = Sl.QuestionValuationRepo.GetByQuestionIds(questions.GetIds(), user.Id);

        foreach (var question in questions)
        {
            CreateOrUpdateValuation(question, questionValuations.ByQuestionId(question.Id), user.Id, relevance);
            ChangeTotalInOthersWishknowledge(relevance==50, user.Id, question);
            _nhibernateSession.CreateSQLQuery(GenerateRelevancePersonal(question.Id)).ExecuteUpdate();

            ProbabilityUpdate_Valuation.Run(question, user, _nhibernateSession, _questionRepo);
        }
        UpdateTotalRelevancePersonalInCache(questions);
        SetUserWishCountQuestions(user.Id,_sessionUser);

        var creatorGroups = questions.Select(q => new UserTinyModel(q.Creator)).GroupBy(c => c.Id);
        foreach (var creator in creatorGroups)
            _reputationUpdate.ForUser(creator.First());
    }

    private void UpdateRelevancePersonal(int questionId, int userId, int relevance = 50)
    {
        var question = EntityCache.GetQuestionById(questionId);
        ChangeTotalInOthersWishknowledge(relevance == 50, userId, question);
        CreateOrUpdateValuation(questionId, userId, relevance);

        SetUserWishCountQuestions(userId, _sessionUser);

       
        _nhibernateSession.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        _nhibernateSession.Flush();

        _reputationUpdate.ForQuestion(questionId);

        if (relevance != -1)
            ProbabilityUpdate_Valuation.Run(questionId, userId, _nhibernateSession, _questionRepo);
    }

    public void SetUserWishCountQuestions(int userId, SessionUser sessionUser)
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
            GenerateAvgQuery("TotalQuality", "Quality", questionId) ;
    }

    private static string GenerateRelevancePersonal(int questionId)
    {
        return
            GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionId) + " " +
            GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionId);
    }

    public static void UpdateTotalRelevancePersonalInCache(IList<QuestionCacheItem> questions)
    {
        var questionValuations = Sl.QuestionValuationRepo.GetByQuestionsFromCache(questions);
        foreach (var question in questions)
        {
            var totalRelevancePersonalEntriesCount = questionValuations.Count(v => v.Question.Id == question.Id && v.IsInWishKnowledge);
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
                    " WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                "WHERE Id = " + questionId + ";";
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId)
    {
        return "UPDATE Question SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM QuestionValuation " +
                    "WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + questionId + ";";
    }

    private void CreateOrUpdateValuation(int questionId, int userId, int relevancePersonal = -2)
    {
        var questionValuation = Sl.QuestionValuationRepo.GetBy(questionId, userId);
        var question = EntityCache.GetQuestion(questionId);

        CreateOrUpdateValuation(question, questionValuation, userId, relevancePersonal);
    }

    private void CreateOrUpdateValuation(
        QuestionCacheItem question, 
        QuestionValuation questionValuation, 
        int userId, 
        int relevancePersonal = -2)
    {
        var questionValuationRepo = Sl.QuestionValuationRepo;

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = _questionRepo.GetById(question.Id),
                User = Sl.UserRepo.GetById(userId),
                RelevancePersonal = relevancePersonal,
                CorrectnessProbability = question.CorrectnessProbability
            };

            questionValuationRepo.Create(newQuestionVal); ;
        }
        else
        {
            if (relevancePersonal != -2)
                questionValuation.RelevancePersonal = relevancePersonal;

            questionValuationRepo.Update(questionValuation);
        }
        questionValuationRepo.Flush();
    }
}