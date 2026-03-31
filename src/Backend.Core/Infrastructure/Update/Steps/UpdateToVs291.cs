using NHibernate;

public class UpdateToVs291
{
    public static void Run(ISession session)
    {
        // Align collation of ai_usage_log to utf8mb4_unicode_ci
        // to match aimodelwhitelist (created with utf8mb4_unicode_ci in UpdateToVs289).
        // The JOIN on ai_usage_log.Model = aimodelwhitelist.ModelId fails
        // when collations differ (utf8mb4_0900_ai_ci vs utf8mb4_unicode_ci).
        session.CreateSQLQuery(
            @"ALTER TABLE ai_usage_log
              CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci"
        ).ExecuteUpdate();
    }
}
