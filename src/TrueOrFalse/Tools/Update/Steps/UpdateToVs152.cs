using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs152
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

