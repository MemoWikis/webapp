
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs212
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            session
                .CreateSQLQuery(
                    @"ALTER TABLE `memucho`.`user` ADD COLUMN `LearningSessionOptions` VARCHAR(500) NULL AFTER `TotalInOthersWishknowledge`;"
                ).ExecuteUpdate();
            
        }
    }
}