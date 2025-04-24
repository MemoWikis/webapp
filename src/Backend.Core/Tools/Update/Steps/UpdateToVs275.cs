using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs275
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE question DROP COLUMN TotalViews;"
            ).ExecuteUpdate();
    }
}