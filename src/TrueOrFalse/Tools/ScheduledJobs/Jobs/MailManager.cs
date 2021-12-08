using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RollbarSharp;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class MailManager : IJob //scheduledMailTransmitter
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfulJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetAllMailMessages();

                //increase interval when mail jobs exist
                if (jobs.Count > 0)
                {
                    var newTrigger = TriggerBuilder.Create()
                        .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMilliseconds(100))
                            .RepeatForever()).Build();
                    var oldTrigger = context.Trigger;
                    var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.RescheduleJob(oldTrigger.Key, newTrigger);
                }

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
                            successfulJobIds.Add(mailMessageJob.Id);
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
                if (successfulJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfulJobIds);
                    Logg.r().Information("Job MailManager send " + successfulJobIds.Count + " mails.");
                    successfulJobIds.Clear();
                }

                //decrease interval when no mail job exist
                if (scope.R<JobQueueRepo>().GetAllMailMessages().Count == 0)
                {
                    var newTrigger = TriggerBuilder.Create()
                        .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMilliseconds(1000))
                            .RepeatForever()).Build();
                    var oldTrigger = context.Trigger;
                    var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.RescheduleJob(oldTrigger.Key, newTrigger);
                }
            }, "MailManager");
        }
    }
}
