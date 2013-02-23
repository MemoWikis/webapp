using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs018
    {
        public static void Run()
        {
            ServiceLocator.Resolve<ISession>()
                .CreateSQLQuery("ALTER TABLE `trueorfalse`.`imagemetadata` CHANGE COLUMN `LicenceInfo` `LicenceInfo` VARCHAR(8000) NULL DEFAULT NULL")
                .ExecuteUpdate();
        }
    }
}
