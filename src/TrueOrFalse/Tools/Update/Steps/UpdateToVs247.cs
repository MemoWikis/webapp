using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs247
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE answerfeature;"
            ).ExecuteUpdate();
    }
}