using NHibernate;

namespace TrueOrFalse.Updates;

internal class UpdateToVs255
{
    public static void Run(ISession nhibernateSession)
    {
        nhibernateSession
            .CreateSQLQuery(
                @"ALTER TABLE questionview DROP FOREIGN KEY FK_questionview_game_round;"
            ).ExecuteUpdate();

        nhibernateSession
            .CreateSQLQuery(
                @"DROP TABLE game_round;"
            ).ExecuteUpdate();
    }
}