using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    [DisallowConcurrentExecution]
    public class RecalcReputation : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var userIds = scope.R<JobQueueRepo>().GetReputationUpdateUsers(); //nee, weil hier brauche ich auch die IDs der Tabellenzeilen, um die erfolgreichen nachher löschen zu können
                //grouped by
                //var uniqueUserIds = userIds
                //foreach (var userId in uniqueUserIds)
                //    {
                //        Run(Sl.R<UserRepo>().GetById(userId));
                //    }
                //check in table "jobs" if reputation should be recalculated for users. group them by userid, then do it.
                //mit try catch finally; bei catch Fehler in rollbar loggen; im finally die erfolgreichen löschen
            }, "RecalcReputation");
        }

    }
}
