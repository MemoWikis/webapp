using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs093
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `reference`
	                CHANGE COLUMN `AdditionalInfo` `AdditionalInfo` TEXT NULL DEFAULT NULL AFTER `ReferenceType`,
	                CHANGE COLUMN `ReferenceText` `ReferenceText` TEXT NULL DEFAULT NULL AFTER `AdditionalInfo`;"
            ).ExecuteUpdate();

        }
    }
}