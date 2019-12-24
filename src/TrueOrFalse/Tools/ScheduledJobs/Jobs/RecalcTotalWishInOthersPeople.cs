using NHibernate;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RecalcTotalWishInOthersPeople : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                Sl.Resolve<ISession>().CreateSQLQuery(
                        @"UPDATE user SET TotalInOthersWishknowledge = (
                        SELECT count(*) FROM questionvaluation qv
                        JOIN question q
                        ON qv.Questionid = q.Id
                        WHERE qv.RelevancePersonal > 0
                        AND q.Creator_id = user.Id
                        AND qv.UserId <> user.Id);"
                    ).ExecuteUpdate();

            }, "RecalcTotalWishInOthersPeople");
        }
    }
}