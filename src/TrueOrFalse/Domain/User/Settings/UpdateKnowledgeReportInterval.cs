using System;
using System.Text.RegularExpressions;
using Seedworks.Lib;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class UpdateKnowledgeReportInterval
{

    public const string CommandName = "kri";
    public const string ExpirationDateFormat = "yyyy-MM-dd";


    public static UIMessage Run(string updateCommand, string token)
    {
        var commandParts = Regex.Split(updateCommand, "__");

        UserSettingNotificationInterval knowledgeReportInterval;
        DateTime expirationDate;
        User user;

        try
        {
            if (commandParts[0] != CommandName)
                throw new Exception("Command is not \"" + CommandName + "\", but: " + commandParts[0]);

            user = Sl.R<UserRepo>().GetById(commandParts[2].ToInt());
            if (user == null)
                throw new Exception("User is not defined, id is " + commandParts[2]);

            if (!UpdateSetting.IsValidUpdateCommand(user, updateCommand, token))
            {
                Logg.r().Error("UpdateCommand for userId=" + user.Id + " is invalid, might have been manipulated. Command: " + updateCommand + " -- Token: " + token);
                throw new Exception("UpdateCommand does not fit token, command might have been manipulated");
            }
                
            if (!Enum.IsDefined(typeof(UserSettingNotificationInterval), (UserSettingNotificationInterval)commandParts[1].ToInt()))
                throw new Exception("Undefined int value for KnowledgeReportInterval: " + commandParts[1]);

            knowledgeReportInterval = (UserSettingNotificationInterval)commandParts[1].ToInt();

            expirationDate = DateTime.ParseExact(commandParts[3], ExpirationDateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception exception)
        {
            Logg.r().Information("UpdateKnowledgeReportInterval could not change settings. The exception is: " + exception.Message);
            return new ErrorMessage("Die Einstellung konnte nicht verändert werden, da der übermittelte Link fehlerhaft war.");
        }

        if (expirationDate.Date < DateTime.Now.Date)
            return new ErrorMessage("Die Einstellung konnte NICHT übernommen werden, da die Gültigkeit des Links abgelaufen ist. <br>" +
                                    "Du kannst die Einstellung jederzeit hier ändern, wenn du eingeloggt bist.");

        user.KnowledgeReportInterval = knowledgeReportInterval;
        Sl.R<SessionUser>().User.KnowledgeReportInterval = knowledgeReportInterval;
        Sl.R<UserRepo>().Update(user);

        var intervalWord = "";
        switch (knowledgeReportInterval)
        {
            case UserSettingNotificationInterval.Daily:
                intervalWord = "täglich";
                break;
            case UserSettingNotificationInterval.Weekly:
                intervalWord = "wöchentlich";
                break;
            case UserSettingNotificationInterval.Monthly:
                intervalWord = "monatlich";
                break;
            case UserSettingNotificationInterval.Quarterly:
                intervalWord = "vierteljährlich";
                break;
            case UserSettingNotificationInterval.NotSet:
                break;
            case UserSettingNotificationInterval.Never:
                intervalWord = "nicht mehr";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(knowledgeReportInterval), knowledgeReportInterval, null);
        }


        return new SuccessMessage("<p><strong>Deine Einstellung wurde aktualisiert!</strong> </p>" +
                                  "<p>Dein Wissensbericht wird jetzt <strong>" + intervalWord + "</strong> an deine E-Mail-Adresse " + user.EmailAddress + " versendet.<br>" +
                                    "Du kannst die Einstellung jederzeit hier ändern, wenn du eingeloggt bist.</p>");
    }


    public static string GetUpdateCommand(User user, UserSettingNotificationInterval knowledgeReportInterval,
        DateTime expirationDate)
    {
        return CommandName + "__" + (int)knowledgeReportInterval + "__" + user.Id + "__" + expirationDate.ToString(ExpirationDateFormat);
    }

    public static string GetHash(User user, string updateCommand)
    {
        return UpdateSetting.HashUpdateCommand(user, updateCommand);
    }


    public static string GetFullHtmlLinkForInterval(User user, UserSettingNotificationInterval interval)
    {
        var intervalWord = "";
        switch (interval)
        {
            case UserSettingNotificationInterval.Daily:
                intervalWord = "täglich";
                break;
            case UserSettingNotificationInterval.Weekly:
                intervalWord = "wöchentlich";
                break;
            case UserSettingNotificationInterval.Monthly:
                intervalWord = "monatlich";
                break;
            case UserSettingNotificationInterval.Quarterly:
                intervalWord = "vierteljährlich";
                break;
            case UserSettingNotificationInterval.NotSet:
                break;
            case UserSettingNotificationInterval.Never:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(interval), interval, null);
        }

        var update = GetUpdateCommand(user, interval, DateTime.Now.AddMonths(1));
        var token = GetHash(user, update);
        return "<a href=\"" + Settings.CanonicalHost + Links.UserSettings() + "?update=" + update + "&token=" + token + "\">" + intervalWord + "</a>";
    }

    public static string GetFullHtmlLinkForSignOut(User user)
    {
        var update = GetUpdateCommand(user, UserSettingNotificationInterval.Never, DateTime.Now.AddMonths(1));
        var token = GetHash(user, update);
        return "<a href=\"" + Settings.CanonicalHost + Links.UserSettings() + "?update=" + update + "&token=" + token + "\">E-Mails abbestellen</a>";
    }
}
