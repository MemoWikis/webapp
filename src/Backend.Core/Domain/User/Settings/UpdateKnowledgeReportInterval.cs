﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class UpdateKnowledgeReportInterval
{
    public const string CommandName = "kri";
    public const string ExpirationDateFormat = "yyyy-MM-dd";

    public static UpdateKnowledgeReportIntervalResult Run(int userId,
        int val,
        string expires,
        string token,
        UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        UserSettingNotificationInterval knowledgeReportInterval;
        DateTime expirationDate;
        User user;

        try
        {
            user = userReadingRepo.GetById(userId);
            if (user == null)
                throw new Exception("User is not defined, id is " + userId);

            if (!IsValidCommand(user, 
                    val, 
                    expires, 
                    token,
                    httpContextAccessor.HttpContext,
                    webHostEnvironment))
                throw new Exception("UpdateCommand not valid, might have been manipulated. UserId=" + user.Id + "; val=" + val + "; expires=" + expires + "; token=" + token);

            if (!Enum.IsDefined(typeof(UserSettingNotificationInterval), (UserSettingNotificationInterval)val))
                throw new Exception("Undefined int value for KnowledgeReportInterval: " + val);

            knowledgeReportInterval = (UserSettingNotificationInterval)val;

            expirationDate = DateTime.ParseExact(expires, ExpirationDateFormat, System.Globalization.CultureInfo.InvariantCulture);

            if (expirationDate.Date < DateTime.Now.Date)
            {
                Log.Information("UpdateKnowledgeReportInterval could not change settings because link already expired.");
                result.ResultMessage = new ErrorMessage("Die Einstellung konnte NICHT übernommen werden, da die Gültigkeit des Links abgelaufen ist. <br>" +
                                                        "Du kannst die Einstellung jederzeit hier ändern, wenn du eingeloggt bist.");
                return result;
            }

            return Run(user, knowledgeReportInterval, result, userWritingRepo);
        }
        catch (Exception exception)
        {
            Log.Error("UpdateKnowledgeReportInterval could not change settings. The exception is: " + exception.Message);
            result.ResultMessage = new ErrorMessage("Die Einstellung konnte nicht verändert werden, da der übermittelte Link fehlerhaft war.");
            return result;
        }

    }

    public static UpdateKnowledgeReportIntervalResult Run(User user,
        UserSettingNotificationInterval knowledgeReportInterval,
        UpdateKnowledgeReportIntervalResult result,
        UserWritingRepo userWritingRepo)
    {
        user.KnowledgeReportInterval = knowledgeReportInterval;
        userWritingRepo.Update(user);

        result.Success = true;
        result.AffectedUser = user;
        result.ResultMessage = new SuccessMessage("<p><strong>Deine Einstellung wurde aktualisiert!</strong> </p>" +
                                                  "<p>Dein Wissensbericht wird jetzt <strong>" + GetIntervalAsString(knowledgeReportInterval, "nicht mehr") + "</strong> " +
                                                  "an deine E-Mail-Adresse " + user.EmailAddress + " versendet.<br>" +
                                                  "Du kannst die Einstellung jederzeit hier ändern, wenn du eingeloggt bist.</p>");
        return result;
    }


    public static string GetHash(User user, int val,
        string expires,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var updateCommand = CommandName + val + user.Id + expires;
        return UpdateSetting.HashUpdateCommand(user,
            updateCommand,
            httpContextAccessor.HttpContext,
            webHostEnvironment);
    }

    public static bool IsValidCommand(User user,
        int val,
        string expires,
        string token,
        HttpContext httpContext,
        IWebHostEnvironment webHostEnvironment)
    {
        var updateCommand = CommandName + val + user.Id + expires;
        return UpdateSetting.IsValidUpdateCommand(user,
            updateCommand,
            token,
            httpContext, 
            webHostEnvironment);
    }

    public static string GetLinkParamsForInterval(User user,
        UserSettingNotificationInterval interval,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        var expires = DateTime.Now.Date.AddMonths(1).ToString(ExpirationDateFormat);
        var update = "update=" + CommandName + "&val=" + (int)interval + "&userId=" + user.Id + "&expires=" + expires;
        var token = GetHash(user, 
            (int)interval, 
            expires, 
            httpContextAccessor,
            webHostEnvironment);
        return update + "&token=" + token;
    }

    public static string GetIntervalAsString(UserSettingNotificationInterval interval, string wordForNever = "nie")
    {
        switch (interval)
        {
            case UserSettingNotificationInterval.NotSet:
                goto case UserSettingNotificationInterval.Weekly;  //defines standard behaviour
            case UserSettingNotificationInterval.Daily:
                return "täglich";
            case UserSettingNotificationInterval.Weekly:
                return "wöchentlich";
            case UserSettingNotificationInterval.Monthly:
                return "monatlich";
            case UserSettingNotificationInterval.Quarterly:
                return "vierteljährlich";
            case UserSettingNotificationInterval.Never:
                return wordForNever;
            default:
                throw new ArgumentOutOfRangeException(nameof(interval), interval, null);
        }

    }
}
