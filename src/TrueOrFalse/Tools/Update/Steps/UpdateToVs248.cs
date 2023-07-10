using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs248
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE answerfeature_to_answer;"
            ).ExecuteUpdate();
    }
}