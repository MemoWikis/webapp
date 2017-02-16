using System.Linq;
using NHibernate;

public static class SetInKnowledge 
{
    public static void Pin(int setId, User user) => UpdateRelevancePersonal(setId, user, 50);

    public static void Unpin(int setId, User user) => UpdateRelevancePersonal(setId, user, -1);

    public static void UnpinQuestionsInSet(int setId, User user)
    {
        var questionsInUnpinnedSet = Sl.Resolve<SetRepo>().GetById(setId).QuestionsInSet.Select(x => x.Question).ToList();
        var ids = questionsInUnpinnedSet.Select(q => q.Id).ToList();

        if (ids.Count == 0) return;

        var query = $@"
            select q.Question_id from
                user u
                join setvaluation sv
                on u.Id = sv.UserId
                join questionset s
                on sv.SetId = s.Id
                join questioninset q
                on s.Id = q.Set_id
                where u.Id = {user.Id}
                and sv.SetId != {setId} 
                and sv.RelevancePersonal >= 0
                and q.Question_id in ({ids.Select(x => x.ToString())
            .Aggregate((a, b) => a + ", " + b)})";

        var questionsInOtherPinnedSetsIds = Sl.Resolve<ISession>().CreateSQLQuery(query).List<int>();

        foreach (var question in questionsInUnpinnedSet.Where(question => questionsInOtherPinnedSetsIds.All(id => id != question.Id)))
        {
            QuestionInKnowledge.Unpin(question.Id, user);
        }
    }

    private static void UpdateRelevancePersonal(int setId, User user, int relevance = 50)
    {
        
        Sl.R<CreateOrUpdateSetValuation>().Run(setId, user.Id, relevancePeronal: relevance);

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