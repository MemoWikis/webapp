using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs149
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `question`
	                CHANGE COLUMN `Solution` `Solution` VARCHAR(3000) NULL DEFAULT NULL AFTER `Creator_id`,
	                CHANGE COLUMN `SolutionMetadataJson` `SolutionMetadataJson` VARCHAR(7000) NULL DEFAULT NULL AFTER `SolutionType`;"
            ).ExecuteUpdate();
        }
    }
}