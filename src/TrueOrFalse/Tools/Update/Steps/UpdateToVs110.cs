using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs110
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(@"CREATE TABLE `useractivity` (
	                                `Id` INT(11) NOT NULL AUTO_INCREMENT,
	                                `At` DATETIME NULL DEFAULT NULL,
	                                `Type` INT(11) NULL DEFAULT NULL,
	                                `DateCreated` DATETIME NULL DEFAULT NULL,
	                                `DateModified` DATETIME NULL DEFAULT NULL,
	                                `UserConcerned_id` INT(11) NULL DEFAULT NULL,
	                                `Question_id` INT(11) NULL DEFAULT NULL,
	                                `Set_id` INT(11) NULL DEFAULT NULL,
	                                `Category_id` INT(11) NULL DEFAULT NULL,
	                                `Date_id` INT(11) NULL DEFAULT NULL,
	                                `Game_id` INT(11) NULL DEFAULT NULL,
	                                `UserIsFollowed_id` INT(11) NULL DEFAULT NULL,
	                                `UserCauser_id` INT(11) NULL DEFAULT NULL,
	                                PRIMARY KEY (`Id`),
	                                INDEX `UserConcerned_id` (`UserConcerned_id`),
	                                INDEX `Question_id` (`Question_id`),
	                                INDEX `Set_id` (`Set_id`),
	                                INDEX `Category_id` (`Category_id`),
	                                INDEX `Date_id` (`Date_id`),
	                                INDEX `Game_id` (`Game_id`),
	                                INDEX `UserIsFollowed_id` (`UserIsFollowed_id`),
	                                INDEX `UserCauser_id` (`UserCauser_id`)
                                )
                                COLLATE='utf8_general_ci'
                                ENGINE=InnoDb;"
            ).ExecuteUpdate();
        }
    }
}