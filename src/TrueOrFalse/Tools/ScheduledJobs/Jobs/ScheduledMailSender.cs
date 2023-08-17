using System.Net.Mail;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Rollbar;

namespace TrueOrFalse.Tools.ScheduledJobs.Jobs
{
    class ScheduledMailSender : IJob
    {
        private const string CacheKey = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ScheduledMailSender(IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobExecute.Run(async scope =>
            {
                var job = scope.Resolve<JobQueueRepo>().GetTopPriorityMailMessage();

                var successfulJobIds = _cache.Get(CacheKey) as List<int> ?? new List<int>();

                //increase interval when no mail job exist
                if (job == null)
                {
                    if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) ==
                        context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(1000))
                    {
                       await SetJobInterval(1000, context);
                    }

                    return;
                }

                //decrease interval when mail job exist
                if (context.Trigger.GetFireTimeAfter(context.Trigger.GetPreviousFireTimeUtc()) !=
                    context.Trigger.GetPreviousFireTimeUtc() + TimeSpan.FromMilliseconds(100))
                {
                    await SetJobInterval(100, context);
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
                        _cache.Set(CacheKey, successfulJobIds, DateTimeOffset.MaxValue);
                    }
                    else
                    {
                        var e = new Exception(job.JobContent + job.Id);
                        new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in job ScheduledMailSender. MailMessage was null.");
                        RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
                    }
                }
                catch (Exception e)
                {
                    new Logg(_httpContextAccessor, _webHostEnvironment).r().Error(e, "Error in job ScheduledMailSender.");
                    RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(e));
                }

                var currentJobId = new List<int>();
                currentJobId.Add(job.Id);

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
}
