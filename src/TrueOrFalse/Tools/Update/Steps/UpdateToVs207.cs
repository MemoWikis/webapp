using TrueOrFalse;
using RabbitMQ.Client.Impl;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs207
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            session
                .CreateSQLQuery(
                    @"ALTER TABLE `memucho`.`user` ADD COLUMN `TotalInOthersWishknowledge` INT NULL AFTER `DateModified`;"
                ).ExecuteUpdate();

            session
                .CreateSQLQuery(
                    @"UPDATE user SET TotalInOthersWishknowledge = (
                        SELECT count(*) FROM questionvaluation qv
                        JOIN question q
                        ON qv.Questionid = q.Id
                        WHERE qv.RelevancePersonal > 0
                        AND q.Creator_id = user.Id
                        AND qv.UserId <> user.Id);"
                ).ExecuteUpdate();
        }
    }
}