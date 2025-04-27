using NHibernate;

internal class UpdateToVs239
{
    public static void Run(ISession session)
    {
        session
            .CreateSQLQuery(
                @"ALTER TABLE user ADD stripeId VARCHAR(255);"
            ).ExecuteUpdate();
    }
}