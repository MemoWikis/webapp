using NHibernate;

public class CategoryInKnowledge
{
    public static void Pin(int categoryId, User user) => UpdateRelevancePersonal(categoryId, user, 50);

    public static void Unpin(int categoryId, User user) => UpdateRelevancePersonal(categoryId, user, -1);

    private static void UpdateRelevancePersonal(int categoryId, User user, int relevance = 50)
    {
        CreateOrUpdateCategoryValuation.Run(categoryId, user.Id, relevancePeronal: relevance);

        var session = Sl.R<ISession>();
        session.CreateSQLQuery(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", categoryId)).ExecuteUpdate();
        session.Flush();

        ReputationUpdate.ForCategory(categoryId);
    }

    private static string GenerateEntriesQuery(string fieldToSet, string fieldSource, int categoryId)
    {
        return "UPDATE Category SET " + fieldToSet + "Entries = " +
                    "(SELECT COUNT(Id) FROM CategoryValuation " +
                    "WHERE CategoryId = " + categoryId + " AND " + fieldSource + " != -1) " +
                "WHERE Id = " + categoryId + ";";
    }
}