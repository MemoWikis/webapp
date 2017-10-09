using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs185
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `questionset`
	                ADD COLUMN `CopiedFrom` INT(11) NULL DEFAULT NULL AFTER `Creator_id`;"
                ).ExecuteUpdate();
        }
    }
}