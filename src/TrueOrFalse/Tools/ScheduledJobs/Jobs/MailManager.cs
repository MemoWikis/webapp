using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core.Lifetime;
using Newtonsoft.Json;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class MailManager : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                List<int> successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetAllMailMessages();
                var jobsByMailPriority = jobs.OrderByDescending(j => JsonConvert.DeserializeObject<MailMessageJob>(j.JobContent)?.Priority);

                foreach (var mailMessageJob in jobsByMailPriority)
                {
                    try
                    {
                        var currentMailMessage = JsonConvert.DeserializeObject<MailMessageJob>(mailMessageJob.JobContent)?.MailMessage;
                        var smtpClient = new SmtpClient();
                        if (currentMailMessage != null)
                        {
                            smtpClient.Send(currentMailMessage);
                            successfullJobIds.Add(mailMessageJob.Id);
                        }
                        else
                        {
                            var e = new Exception(mailMessageJob.JobContent + mailMessageJob.Id);
                            Logg.r().Error(e, "Error in job MailManager. MailMessage was null.");
                            new RollbarClient().SendException(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job MailManager.");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count <= 0) return;
                scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                Logg.r().Information("Job MailManager send " + successfullJobIds.Count + " mails.");
                successfullJobIds.Clear();
            }, "MailManager");
        }
    }
}
