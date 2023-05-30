using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs245
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE answer_test;"
            ).ExecuteUpdate();
    }
}