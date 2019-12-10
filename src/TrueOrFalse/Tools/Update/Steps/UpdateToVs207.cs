using TrueOrFalse;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs207
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `memucho`.`user` ADD COLUMN `TotalInOthersWishknowledge` INT NULL AFTER `DateModified`;"
                ).ExecuteUpdate();
        }
    }
}