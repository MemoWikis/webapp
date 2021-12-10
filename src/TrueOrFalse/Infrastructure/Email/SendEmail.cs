using System.Net.Mail;
using Newtonsoft.Json;

public class SendEmail
{
    public static void Run(MailMessage mailMessage, MailMessagePriority priority)
    {
        if (Sl.UserRepo.GetByEmail(mailMessage.To.ToString()).BouncedMail)
        {
            return;
        }

        MailMessageJob mail = new MailMessageJob() { MailMessage = mailMessage, Priority = priority};

        Sl.JobQueueRepo.Add(JobQueueType.MailMessage, JsonConvert.SerializeObject(mail));
    }
}