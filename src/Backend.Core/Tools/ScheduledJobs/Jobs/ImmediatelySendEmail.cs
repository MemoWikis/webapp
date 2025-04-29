using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Quartz;
using System.Net.Mail;

public class ImmediatelySendEmail(IMemoryCache cache)
    : IJob
{
    private const string CacheKey = "SuccessfulMailJobs-19B1DFD8-0DA3-4B69-BFC8-08095EEEFB08";

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var currentMailMessageJson = JsonConvert.DeserializeObject<MailMessageJson>(dataMap.GetString("mailJsonString"));

        Log.Information("Job Started - SendMail");

        var currentMailMessage = new MailMessage(
            currentMailMessageJson.From,
            currentMailMessageJson.To,
            currentMailMessageJson.Subject,
            currentMailMessageJson.Body);

        var smtpClient = new SmtpClient("host.docker.internal");

        try
        {
            smtpClient.Send(currentMailMessage);
        }
        catch (Exception e)
        {
            Log.Information("Job failed - SendMail, {e}", e);
        }

        Log.Information("Job Ended - SendMail");

        return Task.CompletedTask;
    }
}