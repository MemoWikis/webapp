using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs122
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"ALTER TABLE `game` ENGINE = InnoDB;").ExecuteUpdate();
            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `useractivity`
	                ADD CONSTRAINT `UserConcerned_id_FK1` FOREIGN KEY (`UserConcerned_id`) REFERENCES `user` (`Id`),
	                ADD CONSTRAINT `Question_id_FK` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`),
	                ADD CONSTRAINT `Set_id_FK` FOREIGN KEY (`Set_id`) REFERENCES `questionset` (`Id`),
	                ADD CONSTRAINT `Category_id_FK` FOREIGN KEY (`Category_id`) REFERENCES `category` (`Id`),
	                ADD CONSTRAINT `Date_id_FK` FOREIGN KEY (`Date_id`) REFERENCES `date` (`Id`),
	                ADD CONSTRAINT `Game_id_FK` FOREIGN KEY (`Game_id`) REFERENCES `game` (`Id`),
	                ADD CONSTRAINT `UserIsFollowed_id` FOREIGN KEY (`UserIsFollowed_id`) REFERENCES `user` (`Id`),
	                ADD CONSTRAINT `UserCauser_id` FOREIGN KEY (`UserCauser_id`) REFERENCES `user` (`Id`);
                ").ExecuteUpdate();
        }
    }
}