using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs177
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `categoryvaluation`
                    ADD COLUMN `CountNotLearned` INT(7) NULL DEFAULT NULL AFTER `RelevancePersonal`,
                    ADD COLUMN `CountNeedsLearning` INT(7) NULL DEFAULT NULL AFTER `CountNotLearned`,
                    ADD COLUMN `CountNeedsConsolidation` INT(7) NULL DEFAULT NULL AFTER `CountNeedsLearning`,
                    ADD COLUMN `CountSolid` INT(7) NULL DEFAULT NULL AFTER `CountNeedsConsolidation`;"
            ).ExecuteUpdate();
        }
    }
}