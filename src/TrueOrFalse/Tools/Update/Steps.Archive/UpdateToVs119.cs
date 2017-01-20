using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs119
    {
        public static void Run()
        {
            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `date`
	                ADD CONSTRAINT `User_id_fk` FOREIGN KEY (`User_id`) REFERENCES `user` (`Id`),
	                ADD CONSTRAINT `TrainingPlan_id_fk_12d` FOREIGN KEY (`TrainingPlan_id`) REFERENCES `trainingplan` (`Id`);").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `trainingplan`
	                ADD CONSTRAINT `Date_id_fk_sdf3` FOREIGN KEY (`Date_id`) REFERENCES `date` (`Id`);").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `trainingdate`
                    ADD CONSTRAINT `TrainingPlan_id_fk2` FOREIGN KEY (`TrainingPlan_id`) REFERENCES `trainingplan` (`Id`);").ExecuteUpdate();

            Sl.R<ISession>().CreateSQLQuery(@"
                ALTER TABLE `trainingdate_question`
                	ADD CONSTRAINT `TrainingDate_id_fk_s23` FOREIGN KEY (`TrainingDate_id`) REFERENCES `trainingdate` (`Id`),
	                ADD CONSTRAINT `Question_id_fk_a23` FOREIGN KEY (`Question_id`) REFERENCES `question` (`Id`);").ExecuteUpdate();
        }
    }
}