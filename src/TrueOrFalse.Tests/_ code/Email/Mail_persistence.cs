using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using NUnit.Framework;
using TrueOrFalse.Utilities.ScheduledJobs;


class Mail_persistence : BaseTest
{
    [Test]

    public void MailSendingTest()
    {
        CleanEmailsFromPickupDirectory.Run();
        var lowPriorityMails = CreateLowPriorityMails();
        var x = SendTestMails(lowPriorityMails);
        var b = x.FindIndex(a => a.Contains("High"));

        Assert.That(x.Count, Is.EqualTo(21));
        Assert.That(b, Is.EqualTo(1));
    }

    public List<MailMessage> CreateLowPriorityMails()
    {
        var mails = new List<MailMessage>();
        int i;
        for (i = 0; i < 20; i++)
        {
            mails.Add(new MailMessage(Settings.EmailFrom,
                Settings.EmailToMemucho,
                "Low Priority Mail",
                "This is a Low Priority Email"));
        }

        return mails;
    }

    public List<string> SendTestMails(List<MailMessage> lowPriorityMails)
    {
        //foreach (var mail in lowPriorityMails)
        //{
        //    SendEmail.Run(mail, MailMessagePriority.Low);
        //}

        var highPriorityMail = new MailMessage(Settings.EmailFrom,
            Settings.EmailToMemucho,
            "High Priority Mail",
            "This is a High Priority Email");


        SendEmail.Run(highPriorityMail, MailMessagePriority.High);
        JobScheduler.Start();
        Thread.Sleep(2000000000);

        var x = GetEmailsFromPickupDirectory.Run().ToList();

        return x;
    }
}
