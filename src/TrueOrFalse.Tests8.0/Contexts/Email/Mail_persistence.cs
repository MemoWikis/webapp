using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using NHibernate.Criterion;
using NUnit.Framework;
using TrueOrFalse.Utilities.ScheduledJobs;


class Mail_persistence : BaseTest
{
    [Test]
    [Ignore("Doesnt work")]
    public void MailSendingTest()
    {
        CleanEmailsFromPickupDirectory.Run();
        var userReadingRepo = R<UserReadingRepo>(); 
        var user = ContextUser.New(R<UserWritingRepo>()).AddWithEmail("ab@c.de").Persist().All.Last();
        var jqr = R<JobQueueRepo>(); 
        SendEmail.Run(CreateLowPriorityMails(user),jqr, userReadingRepo);
        SendEmail.Run(GetHighPriorityMail(user), jqr, userReadingRepo, MailMessagePriority.High);

        JobScheduler.Start(R<RunningJobRepo>());
        Thread.Sleep(1000);
        var mailsInDirectory = GetEmailsFromPickupDirectory.GetAsDateSortedList();

        /*Mails are send delayed (100ms). After 1 second there should be some mails send. Not all!*/
        Assert.That(mailsInDirectory.Count, Is.GreaterThan(4));
        Assert.That(mailsInDirectory.Count, Is.LessThan(12));

        Thread.Sleep(2000);
        mailsInDirectory = GetEmailsFromPickupDirectory.GetAsDateSortedList();
        bool highPriorityMailIsFirst = false;
        using (var sr = new StreamReader(mailsInDirectory[0].FullName))
            highPriorityMailIsFirst = sr.ReadToEnd().Contains("High");

        Assert.That(mailsInDirectory.Count, Is.EqualTo(21));
        Assert.That(highPriorityMailIsFirst, Is.EqualTo(true));
    }

    public List<MailMessage> CreateLowPriorityMails(User user)
    {
        var mails = new List<MailMessage>();

        for (var i = 0; i < 20; i++)
        {
            mails.Add(new MailMessage(
                Settings.EmailFrom,
                user.EmailAddress,
                "Low Priority Mail",
                "This is a Low Priority Email")
            );
        }

        return mails;
    }

    private static MailMessage GetHighPriorityMail(User user)
    {
        var highPriorityMail = new MailMessage(
            Settings.EmailFrom,
            user.EmailAddress,
            "High Priority Mail",
            "This is a High Priority Email"
        );

        return highPriorityMail;
    }

}
