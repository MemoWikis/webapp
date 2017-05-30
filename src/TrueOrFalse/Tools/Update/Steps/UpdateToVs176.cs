using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs176
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `category`
                    ADD COLUMN `CountQuestionsAggregated` INT(7) UNSIGNED ZEROFILL NULL DEFAULT NULL AFTER `AggregatedContentJson`,
                    ADD COLUMN `CountSetsAggregated` INT(7) UNSIGNED ZEROFILL NULL DEFAULT NULL AFTER `CountQuestionsAggregated`;"
            ).ExecuteUpdate();
        }
    }
}