using System;
using System.IO;
using RazorEngine;
using Seedworks.Web.State;
using TrueOrFalse.Frontend.Web.Code;

public class KnowledgeReportMsg
{
    public const string UtmSource = "knowledgeReportEmail";
    public const string UtmCampaignFullString = "";

    public static void SendHtmlMail(User user)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_KnowledgeReport()),
            new KnowledgeReportMsgModel(user, UtmSource)
        );

        var messageTitle = "";
        var intervalWord = "";
        switch (user.KnowledgeReportInterval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case UserSettingNotificationInterval.Weekly; //defines the standard behaviour if setting is not set; needs to be the same as in UserSettings.aspx
            case UserSettingNotificationInterval.Daily:
                messageTitle = "Dein täglicher Wissensbericht";
                intervalWord = "täglich";
                break;
            case UserSettingNotificationInterval.Weekly:
                messageTitle = "Dein wöchentlicher Wissensbericht";
                intervalWord = "wöchentlich";
                break;
            case UserSettingNotificationInterval.Monthly:
                messageTitle = "Dein monatlicher Wissensbericht";
                intervalWord = "monatlich";
                break;
            case UserSettingNotificationInterval.Quarterly:
                messageTitle = "Dein vierteljährlicher Wissensbericht";
                intervalWord = "vierteljährlich";
                break;
        }

        string signOutMessage;
        if (ContextUtil.IsWebContext)
        {
            signOutMessage = "Du erhältst diese E-Mail " + intervalWord + " als Bericht über deinen Wissensstand. " +
                             "Du kannst diese " + UpdateKnowledgeReportInterval.GetFullHtmlLinkForSignOut(user) + " oder die Empfangshäufigkeit ändern (" +
                             UpdateKnowledgeReportInterval.GetFullHtmlLinkForInterval(user, UserSettingNotificationInterval.Daily) + ", " + 
                             UpdateKnowledgeReportInterval.GetFullHtmlLinkForInterval(user, UserSettingNotificationInterval.Weekly) + ", " +
                             UpdateKnowledgeReportInterval.GetFullHtmlLinkForInterval(user, UserSettingNotificationInterval.Monthly) + " oder " +
                             UpdateKnowledgeReportInterval.GetFullHtmlLinkForInterval(user, UserSettingNotificationInterval.Quarterly) +
                             "). " + 
                             "Weitere Einstellungen zu deinen E-Mail-Benachrichtigungen sind in deinen " +
                             "<a href=\"" + Settings.CanonicalHost + Links.UserSettings() + "?utm_medium=email&utm_source=" + UtmSource + UtmCampaignFullString + "&utm_term=editKnowledgeReportSettings\">Konto-Einstellungen</a> " +
                             "möglich.";
        }
        else
        {
            signOutMessage = "In WebContext, you'll find a personalized SignOutMessage here.";
        }


        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            user.EmailAddress,
            "Dein Wissensstand bei memucho",
            parsedTemplate)
            { UserName = user.Name},
            messageTitle: messageTitle,
            signOutMessage: signOutMessage,
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