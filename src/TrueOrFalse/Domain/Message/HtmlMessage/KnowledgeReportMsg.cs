using System;
using System.IO;
using RazorEngine;

public class KnowledgeReportMsg
{
    public const string UtmSource = "knowledgeReportEmail";
    public const string UtmCampaignFullString = "";

    public const string SignOutMessage = "Du erhältst diese E-Mail als Bericht über deinen Wissensstand. " +
                                         "Wenn du diese E-Mails in einem anderen Interval oder gar nicht mehr erhalten möchtest, " +
                                         "ändere die E-Mail-Benachrichtigung bei deinen " +
                                         "<a href=\"https://memucho.de/Nutzer/Einstellungen?utm_medium=email&utm_source=" + UtmSource + UtmCampaignFullString + "&utm_term=editKnowledgeReportSettings\">Konto-Einstellungen</a>.";

    public static void SendHtmlMail(User user)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_KnowledgeReport()), 
            new KnowledgeReportMsgModel(user, UtmSource)
        );

        var messageTitle = "";
        switch (user.KnowledgeReportInterval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case UserSettingNotificationInterval.Weekly; //defines the standard behaviour if setting is not set; needs to be the same as in UserSettings.aspx
            case UserSettingNotificationInterval.Daily:
                messageTitle = "Dein täglicher Wissensbericht";
                break;
            case UserSettingNotificationInterval.Weekly:
                messageTitle = "Dein wöchentlicher Wissensbericht";
                break;
            case UserSettingNotificationInterval.Monthly:
                messageTitle = "Dein monatlicher Wissensbericht";
                break;
            case UserSettingNotificationInterval.Quarterly:
                messageTitle = "Dein vierteljährlicher Wissensbericht";
                break;
        }


        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            user.EmailAddress,
            "Dein Wissensstand bei memucho",
            parsedTemplate)
            { UserName = user.Name},
            messageTitle: messageTitle,
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
        // possible improvement: increase interval that this method is called (every hour) and then
        // check for user's prefered time of day to use memucho, otherwise use standard time (10 a.m.)

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