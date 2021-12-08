using System.Net.Mail;
using Newtonsoft.Json;

public class SendEmail
{
    public static void Run(MailMessage mailMessage, MailMessagePriority priority)
    {
        MailMessageJob mail = new MailMessageJob() { MailMessage = mailMessage, Priority = priority};
        Sl.JobQueueRepo.Add(JobQueueType.MailMessage, JsonConvert.SerializeObject(mail));
    }
}