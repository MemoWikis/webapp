using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs250
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE badge;"
            ).ExecuteUpdate();
    }
}