using NHibernate;

internal class UpdateToVs270
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE category ADD COLUMN TextIsHidden TINYINT(1) DEFAULT 0;"
            ).ExecuteUpdate();
    }
}