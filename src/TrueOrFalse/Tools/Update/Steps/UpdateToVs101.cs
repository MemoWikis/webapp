using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs101
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `category`
	                ADD COLUMN `CorrectnessProbability` INT NULL DEFAULT '50' AFTER `Type`;"
            ).ExecuteUpdate();

        }
    }
}