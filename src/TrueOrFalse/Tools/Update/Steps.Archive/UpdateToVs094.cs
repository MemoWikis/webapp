using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs094
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `question`
	                CHANGE COLUMN `Text` `Text` VARCHAR(3000) NULL DEFAULT NULL AFTER `Id`,
	                CHANGE COLUMN `Solution` `Solution` VARCHAR(1000) NULL DEFAULT NULL AFTER `Creator_id`,
	                CHANGE COLUMN `SolutionMetadataJson` `SolutionMetadataJson` VARCHAR(3000) NULL DEFAULT NULL AFTER `SolutionType`;"
            ).ExecuteUpdate();

        }
    }
}