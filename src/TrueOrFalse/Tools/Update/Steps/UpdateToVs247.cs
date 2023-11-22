using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs247
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
                .CreateSQLQuery(
                    @"DROP TABLE answerfeature;"
                ).ExecuteUpdate();
    }
}