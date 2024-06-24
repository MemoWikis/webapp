using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs271
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE category DROP COLUMN CountQuestions;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE category DROP COLUMN CountSets;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE category DROP COLUMN FormerSetId;"
            ).ExecuteUpdate();
    }
}