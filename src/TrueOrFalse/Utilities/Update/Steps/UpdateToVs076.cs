using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs076
    {
        public static void Run()
        {
            ServiceLocator
                .Resolve<ISession>()
                .CreateSQLQuery(
                    @"RENAME TABLE `gameround` TO `game_round`;
                      RENAME TABLE `games_to_sets` TO `game_to_sets`;
                      DROP TABLE `games_to_users`;"
            ).ExecuteUpdate();
        } 
    }
}