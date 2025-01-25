using Autofac;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Rollbar;
using System.Net.Mail;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs;

class ScheduledMailSender(IMemoryCache cache) : IJob
{
    private const string CacheKey = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";

    public async Task Execute(IJobExecutionContext context)
    {
        Logg.r.Information("Mail Log - before async start");

        await JobExecute.RunAsync(async scope =>
        {
            Logg.r.Information("Mail Log");

            var job = scope.Resolve<JobQueueRepo>().GetTopPriorityMailMessage();

            var successfulJobIds = cache.Get(CacheKey) as List<int> ?? new List<int>();

            //increase interval when no mail job exist
            if (job == null)
            {
                if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) ==
                    context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(5000))
                {
                    await SetJobInterval(5000, context);
                }

                return;
            }

            //decrease interval when mail job exist
            if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) !=
                context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(1000))
            {
                await SetJobInterval(1000, context);
            }

            try
            {
                var currentMailMessageJson = JsonConvert.DeserializeObject<MailMessageJson>(job.JobContent);
                var currentMailMessage = new MailMessage(
                    currentMailMessageJson.From,
                    currentMailMessageJson.To,
                    currentMailMessageJson.Subject,
                    currentMailMessageJson.Body);

                var smtpClient = new SmtpClient("host.docker.internal");

                if (!successfulJobIds.Contains(job.Id))
                {
                    smtpClient.Send(currentMailMessage);
                    successfulJobIds.Add(job.Id);
                    cache.Set(CacheKey, successfulJobIds, DateTimeOffset.MaxValue);
                }
                else
                {
                    var e = new Exception(job.JobContent + job.Id);
                    Logg.r.Error(e, "Error in job ScheduledMailSender. MailMessage was null.");
                    RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
                }
            }
            catch (Exception e)
            {
                Logg.r.Error(e, "Error in job ScheduledMailSender.");
                RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
            }

            var currentJobId = new List<int>(job.Id);

            //Delete job that has been executed
            scope.Resolve<JobQueueRepo>().DeleteById(currentJobId);
        }, "ScheduledMailSender");
    }

    private async Task SetJobInterval(int interval, IJobExecutionContext context)
    {
        var newTrigger = TriggerBuilder.Create()
            .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMilliseconds(interval))
                .RepeatForever()).Build();
        var oldTrigger = context.Trigger;
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.RescheduleJob(oldTrigger.Key, newTrigger);
    }
}

