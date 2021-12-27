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

        SendEmail.Run(CreateLowPriorityMails());
        SendEmail.Run(GetHighPriorityMail(), MailMessagePriority.High);

        JobScheduler.Start();
        Thread.Sleep(220);

        var mailsInDirectory = GetEmailsFromPickupDirectory.Run().ToList();
        var highPriorityMails = mailsInDirectory.FindAll(a => a.Contains("High"));

        Assert.That(mailsInDirectory.Count, Is.EqualTo(21));
        Assert.That(highPriorityMails.Count, Is.EqualTo(1));
    }

    public List<MailMessage> CreateLowPriorityMails()
    {
        var mails = new List<MailMessage>();

        for (var i = 0; i < 20; i++)
        {
            mails.Add(new MailMessage(
                Settings.EmailFrom,
                Settings.EmailToMemucho,
                "Low Priority Mail",
                "This is a Low Priority Email")
            );
        }

        return mails;
    }

    private static MailMessage GetHighPriorityMail()
    {
        var highPriorityMail = new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "High Priority Mail",
            "This is a High Priority Email"
        );

        return highPriorityMail;
    }
}
