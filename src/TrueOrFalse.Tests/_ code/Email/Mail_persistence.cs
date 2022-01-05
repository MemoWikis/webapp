﻿using System;
using System.Collections.Generic;
using System.IO;
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

        var user = createTestUser();

        SendEmail.Run(CreateLowPriorityMails(user));
        SendEmail.Run(GetHighPriorityMail(user), MailMessagePriority.High);

        JobScheduler.Start();
        Thread.Sleep(25000);

        var mailsInDirectory = GetEmailsFromPickupDirectory.GetAsDateSortedList();
        bool highPriorityMailIsFirst = false;
        using (var sr = new StreamReader(mailsInDirectory[0].FullName))
        { 
            highPriorityMailIsFirst = sr.ReadToEnd().Contains("High");
        }
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

    private User createTestUser()
    {
        var user = new User();
        user.Name = "Vorname Nachname";
        user.Birthday = new DateTime(1980, 08, 03);
        user.BouncedMail = false;
        user.EmailAddress = "ab@c.de";
        
        var userRepository = Resolve<UserRepo>();
        userRepository.Create(user);
        return user;
    }

}
