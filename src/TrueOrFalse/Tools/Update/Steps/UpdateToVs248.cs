using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs248
{
    public static void Run()
    {
        Sl.Resolve<ISession>()
            .CreateSQLQuery(
                @"DROP TABLE answerfeature_to_answer;"
            ).ExecuteUpdate();
    }
}