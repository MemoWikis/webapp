using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs141
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questionview`
	                ADD COLUMN `Guid` VARCHAR(36) NULL DEFAULT NULL AFTER `Id`;"
            ).ExecuteUpdate();
        } 
    }
}