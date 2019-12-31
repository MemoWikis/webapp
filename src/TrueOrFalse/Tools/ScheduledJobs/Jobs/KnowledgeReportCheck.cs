using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class KnowledgeReportCheck : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var users = Sl.R<UserRepo>().GetAll().Where(u => u.WishCountQuestions > 1);
                foreach (var user in users)
                {
                    if (KnowledgeReportMsg.ShouldSendToUser(user))
                    {
                        Logg.r().Information("Sending Knowledge-Report to user " + user.Name + " (" + user.Id + ")...");
                        KnowledgeReportMsg.SendHtmlMail(user);
                    }
                }

            }, "KnowledgeReportCheck");
        }

    }
}