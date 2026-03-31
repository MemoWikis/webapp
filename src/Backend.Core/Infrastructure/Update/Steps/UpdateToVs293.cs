using NHibernate;

public class UpdateToVs293
{
    public static void Run(ISession session)
    {
        session.CreateSQLQuery(
            @"ALTER TABLE user MODIFY COLUMN WikiOrder TEXT"
        ).ExecuteUpdate();

        session.CreateSQLQuery(
            @"ALTER TABLE user MODIFY COLUMN FavoriteIds TEXT"
        ).ExecuteUpdate();
    }
}
