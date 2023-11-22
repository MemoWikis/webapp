using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs259
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE questionfeature_to_question;"
            ).ExecuteUpdate();
    }
}