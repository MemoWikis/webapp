using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrueOrFalse;
using ISession = NHibernate.ISession;

public class QuestionInKnowledge : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly ISession _nhibernateSession;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly ProbabilityCalc_Simple1 _probabilityCalcSimple1;
    private readonly AnswerRepo _answerRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public QuestionInKnowledge(SessionUser sessionUser,
        ISession nhibernateSession,
        ReputationUpdate reputationUpdate,
        QuestionReadingRepo questionReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        ProbabilityCalc_Simple1 probabilityCalcSimple1,
        AnswerRepo answerRepo,
         UserReadingRepo userReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _nhibernateSession = nhibernateSession;
        _reputationUpdate = reputationUpdate;
        _questionReadingRepo = questionReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _probabilityCalcSimple1 = probabilityCalcSimple1;
        _answerRepo = answerRepo;
        _userReadingRepo = userReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
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
        if (question.Creator == null || question.Creator(_httpContextAccessor, _webHostEnvironment).Id == userId) 
            return; 
           
        var sign = isIncrement ? "+" : "-" ;
        
                _nhibernateSession
                    .CreateSQLQuery(
                @"Update user Set TotalInOthersWishknowledge = TotalInOthersWishknowledge " + sign + " 1 where id = " +
                question.Creator(_httpContextAccessor, _webHostEnvironment).Id + ";")
                    .ExecuteUpdate();
    }

    private void UpdateRelevancePersonal(IList<QuestionCacheItem> questions, User user, int relevance = 50)
    {
        var questionValuations = _questionValuationRepo.GetByQuestionIds(questions.GetIds(), user.Id);

        foreach (var question in questions)
        {
            CreateOrUpdateValuation(question, questionValuations.ByQuestionId(question.Id), user.Id, relevance);
            ChangeTotalInOthersWishknowledge(relevance==50, user.Id, question);
            _nhibernateSession.CreateSQLQuery(GenerateRelevancePersonal(question.Id)).ExecuteUpdate();

            new ProbabilityUpdate_Valuation(_nhibernateSession,
                    _questionValuationRepo,
                    _probabilityCalcSimple1,
                    _answerRepo,
                    _httpContextAccessor,
                    _webHostEnvironment)
                .Run(question, user, _questionReadingRepo);
        }
        UpdateTotalRelevancePersonalInCache(questions);
        SetUserWishCountQuestions(user.Id,_sessionUser);

        var creatorGroups = questions.Select(q => new UserTinyModel(q.Creator(_httpContextAccessor, _webHostEnvironment))).GroupBy(c => c.Id);
        foreach (var creator in creatorGroups)
            _reputationUpdate.ForUser(creator.First());
    }

    private void UpdateRelevancePersonal(int questionId, int userId, int relevance = 50)
    {
        var question = EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment);
        ChangeTotalInOthersWishknowledge(relevance == 50, userId, question);
        CreateOrUpdateValuation(questionId, userId, relevance);

        SetUserWishCountQuestions(userId, _sessionUser);

       
        _nhibernateSession.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        _nhibernateSession.Flush();

        _reputationUpdate.ForQuestion(questionId, _httpContextAccessor, _webHostEnvironment);

        if (relevance != -1)
            new ProbabilityUpdate_Valuation(_nhibernateSession,
                _questionValuationRepo,
                _probabilityCalcSimple1,
                _answerRepo,
                _httpContextAccessor,
                _webHostEnvironment)
                .Run(questionId, userId, _questionReadingRepo, _userReadingRepo);
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

    public  void UpdateTotalRelevancePersonalInCache(IList<QuestionCacheItem> questions)
    {
        var questionValuations = _questionValuationRepo.GetByQuestionsFromCache(questions);
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
        var questionValuation = _questionValuationRepo.GetBy(questionId, userId);
        var question = EntityCache.GetQuestion(questionId);

        CreateOrUpdateValuation(question, questionValuation, userId, relevancePersonal);
    }

    private void CreateOrUpdateValuation(
        QuestionCacheItem question, 
        QuestionValuation questionValuation, 
        int userId, 
        int relevancePersonal = -2)
    {
        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = _questionReadingRepo.GetById(question.Id),
                User = _userReadingRepo.GetById(userId),
                RelevancePersonal = relevancePersonal,
                CorrectnessProbability = question.CorrectnessProbability
            };

            _questionValuationRepo.Create(newQuestionVal); ;
        }
        else
        {
            if (relevancePersonal != -2)
                questionValuation.RelevancePersonal = relevancePersonal;

            _questionValuationRepo.Update(questionValuation);
        }
        _questionValuationRepo.Flush();
    }
}