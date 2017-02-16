using NHibernate;

public class CategoryInKnowledge
{
    public static void Pin(int categoryId, User user) => UpdateRelevancePersonal(categoryId, user, 50);

    public static void Unpin(int categoryId, User user) => UpdateRelevancePersonal(categoryId, user, -1);

    private static void UpdateRelevancePersonal(int categoryId, User user, int relevance = 50)
    {
        Sl.R<CreateOrUpdateSetValuation>().Run(categoryId, user.Id, relevancePeronal: relevance);

        var session = Sl.R<ISession>();
        session.CreateSQLQuery(GenerateRelevancePersonal(categoryId)).ExecuteUpdate();
        session.Flush();

        ReputationUpdate.ForSet(categoryId);
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