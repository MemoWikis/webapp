using NHibernate;

public class UpdateToVs292
{
    public static void Run(ISession session)
    {
        session.CreateSQLQuery(
            @"ALTER TABLE user ADD COLUMN WikiOrder varchar(255) DEFAULT NULL"
        ).ExecuteUpdate();
    }
}
