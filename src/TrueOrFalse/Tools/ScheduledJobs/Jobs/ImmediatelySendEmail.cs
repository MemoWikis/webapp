using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NHibernate.Cache;
using Quartz;
using System.Net.Mail;

namespace TrueOrFalse.Utilities.ScheduledJobs;

public class ImmediatelySendEmail(IMemoryCache cache)
    : IJob
{
    private const string CacheKey = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var currentMailMessageJson = JsonConvert.DeserializeObject<MailMessageJson>(dataMap.GetString("mailJsonString"));
        var successfulJobIds = cache.Get(CacheKey) as List<int> ?? new List<int>();

        Logg.r.Information("Job Started - SendMail");

        var currentMailMessage = new MailMessage(
            currentMailMessageJson.From,
            currentMailMessageJson.To,
            currentMailMessageJson.Subject,
            currentMailMessageJson.Body);

        var smtpClient = new SmtpClient("host.docker.internal");

        smtpClient.Send(currentMailMessage);
        cache.Set(CacheKey, successfulJobIds, DateTimeOffset.MaxValue);

        Logg.r.Information("Job Ended - SendMail");

        return Task.CompletedTask;
    }
}