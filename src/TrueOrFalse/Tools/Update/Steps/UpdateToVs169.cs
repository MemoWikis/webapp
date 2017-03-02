using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs169
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"CREATE TABLE `categoryvaluation` (
	                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                `UserId` INT(11) NULL DEFAULT NULL,
	                `CategoryId` INT(11) NULL DEFAULT NULL,
	                `RelevancePersonal` INT(11) NULL DEFAULT NULL,
	                `DateCreated` DATETIME NULL DEFAULT NULL,
	                `DateModified` DATETIME NULL DEFAULT NULL,
	                PRIMARY KEY (`Id`)
                )
                COLLATE='utf8_general_ci'
                ENGINE=InnoDb;"
            ).ExecuteUpdate();
        }
    }
}