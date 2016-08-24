using System.Diagnostics;
using System.Text;
using NHibernate;
using Seedworks.Web.State;
using TrueOrFalse;

public static class QuestionInKnowledge
{
    public static void Pin(int questionId, User user)
    {
        UpdateRelevancePersonal(questionId, user);
    }

    public static void Unpin(int questionId, User user)
    {
        UpdateRelevancePersonal(questionId, user, -1);
    }

    public static void Run(QuestionValuation questionValuation)
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

    public static void UpdateQuality(int questionId, int userId, int quality)
    {
        CreateOrUpdateQuestionValue(questionId, userId, quality: quality);

        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(GenerateQualityQuery(questionId)).ExecuteUpdate();
        session.Flush();
    }

    private static void UpdateRelevancePersonal(int questionId, User user, int relevance = 50)
    {
        CreateOrUpdateQuestionValue(questionId, user.Id, relevancePersonal: relevance);

        SetUserWishCountQuestions(user);

        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
        session.Flush();

        if(ContextUtil.IsWebContext)
            AsyncExe.Run(() => { Sl.R<ReputationUpdate>().ForQuestion(questionId);});
        else
            Sl.R<ReputationUpdate>().ForQuestion(questionId);
    
        if (relevance != -1)
            Sl.R<ProbabilityUpdate_Valuation>().Run(questionId, user.Id);
    }

    private static void SetUserWishCountQuestions(User user)
    {
        var query =
            $@"
            UPDATE user 
            SET WishCountQuestions =
                (SELECT count(id)
                FROM QuestionValuation
                WHERE userId = {user
                .Id}
                AND RelevancePersonal > 0) 
            WHERE Id = {user.Id}";
        Sl.Resolve<ISession>().CreateSQLQuery(query).ExecuteUpdate();
    }

    public static void UpdateRelevanceAll(int questionId, int userId, int relevance)
    {
        CreateOrUpdateQuestionValue(questionId, userId, relevanceForAll: relevance);

        var session = Sl.Resolve<ISession>();
        session.CreateSQLQuery(GenerateRelevanceAllQuery(questionId)).ExecuteUpdate();
        session.Flush();            
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

    private static void CreateOrUpdateQuestionValue(int questionId,
                    int userId,
                    int quality = -2,
                    int relevancePersonal = -2,
                    int relevanceForAll = -2)
    {
        QuestionValuationRepo questionValuationRepo = Sl.R<QuestionValuationRepo>();
        var questionValuation = questionValuationRepo.GetBy(questionId, userId);

        if (questionValuation == null)
        {
            var newQuestionVal = new QuestionValuation
            {
                Question = Sl.R<QuestionRepo>().GetById(questionId),
                User = Sl.R<UserRepo>().GetById(userId),
                Quality = quality,
                RelevancePersonal = relevancePersonal,
                RelevanceForAll = relevanceForAll
            };

            questionValuationRepo.Create(newQuestionVal);
        }
        else
        {
            if (quality != -2) questionValuation.Quality = quality;
            if (relevancePersonal != -2) questionValuation.RelevancePersonal = relevancePersonal;
            if (relevanceForAll != -2) questionValuation.RelevanceForAll = relevanceForAll;

            questionValuationRepo.Update(questionValuation);
        }
        questionValuationRepo.Flush();
    }
}