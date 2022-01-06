using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Web;

using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RollbarSharp;
namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class ScheduledMailSender : IJob
    {
        private const string CacheKey = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";
        ObjectCache Cache = MemoryCache.Default;
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var job = scope.R<JobQueueRepo>().GetTopPriorityMailMessage();

                var successfulJobIds = (List<int>)(IEnumerable)Cache.Get(CacheKey) ?? new List<int>();

                //increase interval when no mail job exist
                if (job == null)
                {
                    if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) ==
                        context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(1000))
                    {
                        SetJobInterval(1000, context);
                    }

                    return;
                }

                //decrease interval when mail job exist
                if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) !=
                    context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(100))
                {
                    SetJobInterval(100, context);
                }

                try
                {
                    var currentMailMessageJson = JsonConvert.DeserializeObject<MailMessageJson>(job.JobContent);
                    var currentMailMessage = new MailMessage(currentMailMessageJson.From, currentMailMessageJson.To,
                        currentMailMessageJson.Subject, currentMailMessageJson.Body);
                    var smtpClient = new SmtpClient();
                    if (!successfulJobIds.Contains(job.Id))
                    {
                        smtpClient.Send(currentMailMessage);
                        successfulJobIds.Add(job.Id);
                        Cache.Add(CacheKey, successfulJobIds, DateTimeOffset.MaxValue);
                    }
                    else
                    {
                        var e = new Exception(job.JobContent + job.Id);
                        Logg.r().Error(e, "Error in job ScheduledMailSender. MailMessage was null.");
                        new RollbarClient().SendException(e);
                    }
                }
                catch (Exception e)
                {
                    Logg.r().Error(e, "Error in job ScheduledMailSender.");
                    new RollbarClient().SendException(e);
                }

                var currentJobId = new List<int>();
                currentJobId.Add(job.Id);

                //Delete job that has been executed
                scope.R<JobQueueRepo>().DeleteById(currentJobId);
            }, "ScheduledMailSender");
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
