using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs259
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE questionfeature_to_question;"
            ).ExecuteUpdate();
    }
}