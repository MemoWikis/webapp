using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate;

public static class SetInKnowledge 
{
    public static void Pin(int setId, User user)
    {
        UpdateRelevancePersonal(setId, user, 50);
        PinQuestionsInSet(setId, user);
    }

    private static void PinQuestionsInSet(int setId, User user)
    {
        var questions = Sl.SetRepo.GetById(setId).QuestionsInSet.Select(x => x.Question);
        QuestionInKnowledge.Pin(questions, user);
    }

    public static void Unpin(int setId, User user) => UpdateRelevancePersonal(setId, user, -1);

    public static void UnpinQuestionsInSet(int setId, User user)
    {
        var questionsInUnpinnedSet = Sl.Resolve<SetRepo>().GetById(setId).QuestionsInSet.Select(x => x.Question).ToList();
        var questionIds = questionsInUnpinnedSet.Select(q => q.Id).ToList();

        if (questionIds.Count == 0)
            return;

        var questionsInOtherPinnedSetsIds = ValuatedQuestionsInSets(user, questionIds, exceptSetId: setId);

        foreach (var question in questionsInUnpinnedSet.Where(question => questionsInOtherPinnedSetsIds.All(id => id != question.Id)))
            QuestionInKnowledge.Unpin(question.Id, user);
    }

    public static IList<int> ValuatedQuestionsInSets(User user, IList<int> questionIds, int exceptSetId = -1)
    {
        if (questionIds.IsEmpty())
            return new List<int>();

        Func<int, string> getSetFilter = setId => setId == -1 ? "" : $"and sv.SetId != {setId}";

        var query = $@"
            select 
                q.Question_id 
            from user u
            join setvaluation sv
            on u.Id = sv.UserId
            join questionset s
            on sv.SetId = s.Id
            join questioninset q
            on s.Id = q.Set_id
            where u.Id = {user.Id} 
            {getSetFilter(exceptSetId)} 
            and sv.RelevancePersonal >= 0
            and q.Question_id in ({questionIds.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b)})";

        var questionsInOtherPinnedSetsIds = Sl.Resolve<ISession>().CreateSQLQuery(query).List<int>();
        return questionsInOtherPinnedSetsIds;
    }

    private static void UpdateRelevancePersonal(int setId, User user, int relevance = 50)
    {
        CreateOrUpdateSetValuation.Run(setId, user.Id, relevancePeronal: relevance);

        var session = Sl.R<ISession>();
        session.CreateSQLQuery(GenerateRelevancePersonal(setId)).ExecuteUpdate();
        session.Flush();
            
        ReputationUpdate.ForSet(setId);
    }

    private static string GenerateRelevancePersonal(int setId)
    {
        return
            GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", setId) + " " +
            GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", setId);
    }

    private static string GenerateAvgQuery(string fieldToSet, string fieldSource, int setId)
    {
        return "UPDATE QuestionSet SET " + fieldToSet + "Avg = " +
                    "ROUND((SELECT SUM(" + fieldSource + ") FROM SetValuation " +
                    " WHERE SetId = " + setId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                "WHERE Id = " + setId + ";";
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int setId)
    {
        return "UPDATE QuestionSet SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM SetValuation " +
                    "WHERE SetId = " + setId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + setId + ";";
    }
}