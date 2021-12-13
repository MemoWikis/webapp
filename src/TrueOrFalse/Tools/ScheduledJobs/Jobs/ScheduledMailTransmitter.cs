using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Newtonsoft.Json;
using NHibernate.Criterion;
using Quartz;
using Quartz.Impl;
using RollbarSharp;
namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class ScheduledMailTransmitter : IJob //scheduledMailTransmitter
    {
        private const string SuccessfulMailJobs = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfulJobIds = (List<int>)HttpContext.Current.Cache[SuccessfulMailJobs];
                if (successfulJobIds == null)
                {
                    successfulJobIds = new List<int>();
                    HttpContext.Current.Cache.Insert(SuccessfulMailJobs, successfulJobIds);
                }
                var jobs = scope.R<JobQueueRepo>().GetAllMailMessages();

                //increase interval when mail jobs exist
                if (jobs.Count == 0)
                {
                    if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) ==
                        context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(1000))
                    {
                        SetJobInterval(1000, context);
                    }

                    return;
                }

                if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) ==
                    context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(100))
                {
                    SetJobInterval(100, context);
                }

                var jobsByMailPriority = jobs.OrderByDescending(j => JsonConvert.DeserializeObject<MailMessageJob>(j.JobContent)?.Priority);


                try
                {
                    var currentMailMessage = JsonConvert.DeserializeObject<MailMessageJob>(jobsByMailPriority.FirstOrDefault()?.JobContent)?.MailMessage;
                    var smtpClient = new SmtpClient();
                    if (currentMailMessage != null && !successfulJobIds.Contains(jobsByMailPriority.FirstOrDefault().Id))
                    {
                        smtpClient.Send(currentMailMessage);
                        successfulJobIds.Add(jobsByMailPriority.FirstOrDefault().Id);
                        HttpContext.Current.Cache.Insert(SuccessfulMailJobs, successfulJobIds);
                    }
                    else
                    {
                        var e = new Exception(jobsByMailPriority.First().JobContent + jobsByMailPriority.First().Id);
                        Logg.r().Error(e, "Error in job ScheduledMailTransmitter. MailMessage was null.");
                        new RollbarClient().SendException(e);
                    }
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job ScheduledMailTransmitter.");
                    new RollbarClient().SendException(e);
                }

                //Delete job that has been executed
                scope.R<JobQueueRepo>().DeleteById(successfulJobIds);

            }, "ScheduledMailTransmitter");
        }

        void SetJobInterval(int interval, IJobExecutionContext context)
        {
            var newTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMilliseconds(interval))
                    .RepeatForever()).Build();
            var oldTrigger = context.Trigger;
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.RescheduleJob(oldTrigger.Key, newTrigger);
        }
    }
}
