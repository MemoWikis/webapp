using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse;

public static class QuestionInKnowledge
{
    public static void Pin(int questionId, User user)
    {
        UpdateRelevancePersonal(questionId, user);
    }

    public static void Pin(IList<QuestionCacheItem> questions, User user)
    {
        UpdateRelevancePersonal(questions, user, 50);
    }

    public static void Unpin(int questionId, User user)
    {
        UpdateRelevancePersonal(questionId, user, -1);
    }

    public static void Create(QuestionValuation questionValuation)
    {
        Sl.Resolve<QuestionValuationRepo>().CreateOrUpdate(questionValuation);

        var sb = new StringBuilder();

        sb.Append(GenerateQualityQuery(questionValuation.Question.Id));
        sb.Append(GenerateRelevanceAllQuery(questionValuation.Question.Id));

        sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.Question.Id));
        
        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        session.Flush();
    }

    private static void ChangeTotalInOthersWishknowledge( bool isIncrement, User user, QuestionCacheItem question)
    {
        if (question.Creator == null || question.Creator.Id == user.Id) 
            return; 
           
        var sign = isIncrement ? "+" : "-" ;
        
                Sl.Resolve<ISession>()
                    .CreateSQLQuery(
                @"Update user Set TotalInOthersWishknowledge = TotalInOthersWishknowledge " + sign + " 1 where id = " +
                question.Creator.Id + ";")
                    .ExecuteUpdate();
    }

    private static void UpdateRelevancePersonal(IList<QuestionCacheItem> questions,  User user, int relevance = 50)
    {
        var questionValuations = Sl.QuestionValuationRepo.GetByQuestionIds(questions.GetIds(), user.Id);

        foreach (var question in questions)
        {
            CreateOrUpdateValuation(question, questionValuations.ByQuestionId(question.Id), user, relevance);
            ChangeTotalInOthersWishknowledge(relevance==50, user, question);
            Sl.Session.CreateSQLQuery(GenerateRelevancePersonal(question.Id)).ExecuteUpdate();

            ProbabilityUpdate_Valuation.Run(question, user);
        }
        UpdateTotalRelevancePersonalInCache(questions);
        SetUserWishCountQuestions(user);

        var creatorGroups = questions.Select(q => new UserTinyModel(q.Creator)).GroupBy(c => c.Id);
        foreach (var creator in creatorGroups)
            ReputationUpdate.ForUser(creator.First());
    }

    private static void UpdateRelevancePersonal(int questionId, User user, int relevance = 50)
    {
        var question = EntityCache.GetQuestionById(questionId);
        ChangeTotalInOthersWishknowledge(relevance == 50, user, question);
        CreateOrUpdateValuation(questionId, user, relevance);

        SetUserWishCountQuestions(user);

        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        session.Flush();

        ReputationUpdate.ForQuestion(questionId);

        if (relevance != -1)
            ProbabilityUpdate_Valuation.Run(questionId, user.Id);
    }

    public static void SetUserWishCountQuestions(User user)
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
        Sl.Resolve<ISession>().CreateSQLQuery(query).SetParameter("userId", user.Id).ExecuteUpdate();
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

    private static void UpdateTotalRelevancePersonalInCache(QuestionCacheItem question)
    {
        UpdateTotalRelevancePersonalInCache(new List<QuestionCacheItem>{ question });
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

    private static void CreateOrUpdateValuation(int questionId, User user, int relevancePersonal = -2)
    {
        var questionValuation = Sl.QuestionValuationRepo.GetBy(questionId, user.Id);
        var question = EntityCache.GetQuestion(questionId);

        CreateOrUpdateValuation(question, questionValuation, user, relevancePersonal);
    }

    private static void CreateOrUpdateValuation(
        QuestionCacheItem question, 
        QuestionValuation questionValuation, 
        User user, 
        int relevancePersonal = -2)
    {
        var questionValuationRepo = Sl.QuestionValuationRepo;

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = Sl.QuestionRepo.GetById(question.Id),
                User = user,
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