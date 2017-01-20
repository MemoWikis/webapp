using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs154
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `user`
	                ADD COLUMN `FacebookId` VARCHAR(20) NULL DEFAULT NULL AFTER `CorrectnessProbabilityAnswerCount`,
	                ADD UNIQUE INDEX `FacebookId` (`FacebookId`);"
            ).ExecuteUpdate();
        }
    }
}