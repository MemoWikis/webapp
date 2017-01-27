using System;
using System.IO;
using RazorEngine;

public class KnowledgeReportMsg
{
    public const string UtmSource = "knowledgeReportEmail";
    public const string UtmCampaignFullString = "";

    public const string SignOutMessage = "Du erhältst diese E-Mail als Bericht über deinen Wissensstand. " +
                                         "Wenn du diese E-Mails nicht mehr erhalten möchtest, " +
                                         "deaktiviere die E-Mail-Benachrichtigung bei deinen " +
                                         "<a href=\"https://memucho.de/Nutzer/Einstellungen?utm_medium=email&utm_source=" + UtmSource + UtmCampaignFullString + "&utm_term=editKnowledgeReportSettings\">Konto-Einstellungen</a>.";

    public static void SendHtmlMail(User user)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_KnowledgeReport()), 
            new KnowledgeReportMsgModel(user)
        );

        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            user.EmailAddress,
            "Dein Wissensstand bei memucho",
            parsedTemplate)
            { UserName = user.Name},
            signOutMessage: SignOutMessage,
            utmSource: UtmSource);

        Sl.R<MessageEmailRepo>().Create(new MessageEmail(user, MessageEmailTypes.KnowledgeReport));
        Logg.r().Information("Successfully SENT Knowledge-Report to user " + user.Name + " (" + user.Id + ")");
    }


    /// <summary>
    /// Checks if a user should now get a KnowledgeReport according to his KnowledgeReportInterval-Setting
    /// </summary>
    public static bool ShouldSendToUser(User user)
    {
        // possible improvement: check for user's prefered time of day to use memucho
        // possible improvement: exclude certain times of day if check-interval is increased

        var lastMessageSent = Sl.R<MessageEmailRepo>().GetMostRecentForUserAndType(user.Id, MessageEmailTypes.KnowledgeReport);
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
                shouldHaveSent = DateTime.Now.AddDays(-7).AddHours(1);
                break;
            case UserSettingNotificationInterval.Monthly:
                shouldHaveSent = DateTime.Now.AddMonths(-1).AddHours(1);
                break;
            case UserSettingNotificationInterval.Quarterly:
                shouldHaveSent = DateTime.Now.AddMonths(-3).AddHours(1);
                break;
        }

        Logg.r().Information("KnowledgeReportMsg.ShouldSendToUser: " + user.Name + " - " + lastSentOrRegistered + " <? " + shouldHaveSent);

        return lastSentOrRegistered <= shouldHaveSent;
    }
}