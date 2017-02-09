using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs168
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
              .CreateSQLQuery(
                @"ALTER TABLE `questioninset`
	                ADD COLUMN `Timecode` INT(11) NULL DEFAULT '0' AFTER `Sort`;"
            ).ExecuteUpdate();
        }
    }
}