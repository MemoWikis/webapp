using System;
using System.Collections.Generic;
using System.Net.Mail;
using Newtonsoft.Json;


public class SendEmail
{
    public static void Run(List<MailMessage> lowPriorityMails)
    {
        foreach (var mail in lowPriorityMails)
            Run(mail);
    }

    public static void Run(MailMessage mailMessage, MailMessagePriority priority = MailMessagePriority.Medium)
    {
        if (mailMessage.To.Count > 1)
            throw new Exception("only emails to one user are allowed");

        var user = Sl.UserRepo.GetByEmail(mailMessage.To[0].Address);

        if (user == null)
            throw new Exception("no receiver");

        if (!user.BouncedMail)
            return;

        var mailMessageForJob = new MailMessageJson(mailMessage.From.Address, mailMessage.To[0].Address, mailMessage.Subject, mailMessage.Body);

        Sl.JobQueueRepo.Add(JobQueueType.MailMessage, JsonConvert.SerializeObject(mailMessageForJob), (int)priority);
    }
}