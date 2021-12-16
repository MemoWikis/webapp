using System;
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


        Sl.JobQueueRepo.Add(JobQueueType.MailMessage, JsonConvert.SerializeObject(mailMessage), Convert.ToInt32(priority));
    }
}