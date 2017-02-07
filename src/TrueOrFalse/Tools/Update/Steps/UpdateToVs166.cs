using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs166
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `learningsession`
	                ADD COLUMN `SetsToLearnIdsString` VARCHAR(100) NULL DEFAULT NULL AFTER `SetToLearn_Id`,
                    ADD COLUMN `SetListTitle` VARCHAR(255) NULL DEFAULT NULL AFTER `SetsToLearnIdsString`; "
            ).ExecuteUpdate();
        }
    }
}