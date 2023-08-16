using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RazorLight.Razor;
using RazorLight;

public class KnowledgeReportMsg
{
    private const string UtmSource = "knowledgeReportEmail";

    public static async void SendHtmlMail(User user,
        JobQueueRepo jobQueueRepo,
        MessageEmailRepo messageEmailRepo,
        GetAnswerStatsInPeriod getAnswerStatsInPeriod,
        GetStreaksDays getStreaksDays,
        UserReadingRepo userReadingRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
       
        var template = await File.ReadAllTextAsync(new PathTo(httpContextAccessor, webHostEnvironment).EmailTemplate_KnowledgeReport());
        var model = new KnowledgeReportMsgModel(user,
                UtmSource,
                getAnswerStatsInPeriod,
                getStreaksDays,
                userReadingRepo,
                getUnreadMessageCount,
                knowledgeSummaryLoader,
                questionReadingRepo);

        var project = new FileSystemRazorProject(Path.GetDirectoryName(new PathTo(httpContextAccessor, webHostEnvironment).EmailTemplate_KnowledgeReport()));
        var engine = new RazorLightEngineBuilder()
            .UseProject(project)
            .UseMemoryCachingProvider()
            .Build();
        var parsedTemplate = await engine.CompileRenderStringAsync("templateKey", template, model);

        var messageTitle = "Dein " + (user.KnowledgeReportInterval == UserSettingNotificationInterval.Never ? "" : UpdateKnowledgeReportInterval.GetIntervalAsString(user.KnowledgeReportInterval) + "er") + " Wissensbericht";
        var intervalWord = UpdateKnowledgeReportInterval.GetIntervalAsString(user.KnowledgeReportInterval);

        string signOutMessage = "Du erhältst diese E-Mail " + intervalWord + " als Bericht über deinen Wissensstand. Du kannst diese " +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?" + UpdateKnowledgeReportInterval.GetLinkParamsForInterval(user, UserSettingNotificationInterval.Never) + "\">E-Mails abbestellen</a> " +
                             "oder die Empfangshäufigkeit ändern (" +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?" + UpdateKnowledgeReportInterval.GetLinkParamsForInterval(user, UserSettingNotificationInterval.Daily) + "\">täglich</a>, " +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?" + UpdateKnowledgeReportInterval.GetLinkParamsForInterval(user, UserSettingNotificationInterval.Weekly) + "\">wöchentlich</a>, " +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?" + UpdateKnowledgeReportInterval.GetLinkParamsForInterval(user, UserSettingNotificationInterval.Monthly) + "\">monatlich</a> oder " +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?" + UpdateKnowledgeReportInterval.GetLinkParamsForInterval(user, UserSettingNotificationInterval.Quarterly) + "\">vierteljährlich</a>" +
                             "). " +
                             "Weitere Einstellungen zu deinen E-Mail-Benachrichtigungen sind in deinen " +
                             "<a href=\"" + Settings.CanonicalHost + "/Nutzer/Einstellungen?utm_medium=email&utm_source=" + UtmSource + "&utm_term=editKnowledgeReportSettings\">Konto-Einstellungen</a> " +
                             "möglich.";



        var mailmessage2 =new MailMessage2(
            Settings.EmailFrom,
            user.EmailAddress,
            "Dein Wissensstand bei memucho",
            template);
        mailmessage2.UserName = user.Name; 

       await new HtmlMessage(httpContextAccessor, webHostEnvironment, jobQueueRepo, userReadingRepo)
           .SendAsync(mailmessage2,
            messageTitle,
            signOutMessage,
           UtmSource);

        messageEmailRepo.Create(new MessageEmail(user, MessageEmailTypes.KnowledgeReport));
        Logg.r().Information("Successfully SENT Knowledge-Report to user " + user.Name + " (" + user.Id + ")");
    }


    /// <summary>
    /// Checks if a user should now get a KnowledgeReport according to his KnowledgeReportInterval-Setting
    /// </summary>
    public static bool ShouldSendToUser(User user, MessageEmailRepo messageEmailRepo)
    {

        if ((user.KnowledgeReportInterval == UserSettingNotificationInterval.NotSet) && (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)) // defines standard behaviour if setting is not set
            return false;

        if ((user.KnowledgeReportInterval == UserSettingNotificationInterval.Weekly) && (DateTime.Now.DayOfWeek != DayOfWeek.Sunday))
            return false;

        if ((user.KnowledgeReportInterval == UserSettingNotificationInterval.Monthly) && (DateTime.Now.Day != 1))
            return false;

        DateTime today = DateTime.Now;
        int quarterNumber = (today.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(today.Year, (quarterNumber - 1) * 3 + 1, 1);
        if ((user.KnowledgeReportInterval == UserSettingNotificationInterval.Quarterly) && (DateTime.Now.Date != firstDayOfQuarter.Date))
            return false;

        var lastMessageSent = messageEmailRepo.GetMostRecentForUserAndType(user.Id, MessageEmailTypes.KnowledgeReport);
        var lastSentOrRegistered = lastMessageSent?.DateCreated ?? user.DateCreated;

        var shouldHaveSent = new DateTime();
        switch (user.KnowledgeReportInterval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case UserSettingNotificationInterval.Weekly; //defines the standard behaviour if setting is not set; needs to be the same as in UserSettings.aspx
            case UserSettingNotificationInterval.Daily:
                shouldHaveSent = DateTime.Now.AddHours(-23);
                break;
            case UserSettingNotificationInterval.Weekly:
                shouldHaveSent = DateTime.Now.AddDays(-7).AddHours(2);
                break;
            case UserSettingNotificationInterval.Monthly:
                shouldHaveSent = DateTime.Now.AddMonths(-1).AddHours(2);
                break;
            case UserSettingNotificationInterval.Quarterly:
                shouldHaveSent = DateTime.Now.AddMonths(-3).AddHours(2);
                break;
        }

        Logg.r().Information("KnowledgeReportMsg.ShouldSendToUser: " + user.Name + " - " + lastSentOrRegistered + " <? " + shouldHaveSent);

        return lastSentOrRegistered <= shouldHaveSent;
    }
}