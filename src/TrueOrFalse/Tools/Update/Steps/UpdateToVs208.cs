using TrueOrFalse;
using RabbitMQ.Client.Impl;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using ISession = NHibernate.ISession;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs208
    {
        public static void Run()
        {
            var session = Sl.Resolve<ISession>();

            session
                .CreateSQLQuery(
                    @"ALTER TABLE `user` ADD COLUMN `FollowerCount` INT NULL AFTER `DateModified`;"
                ).ExecuteUpdate();

            session
                .CreateSQLQuery(
                    @"UPDATE user u SET FollowerCount = (
                        SELECT count(*) FROM  user_to_follower uf
                        WHERE uf.User_id =u.id);"
                ).ExecuteUpdate();
        }
    }
}