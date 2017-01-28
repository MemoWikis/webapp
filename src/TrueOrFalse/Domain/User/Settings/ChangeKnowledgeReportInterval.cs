using System;
using System.Text.RegularExpressions;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class ChangeKnowledgeReportInterval
{
    public static string GetHash(string settingString)
    {
        return "agweHkjlsJKLkXsKJLJK";
    }

    public static string GetUpdateCommand(User user, UserSettingNotificationInterval knowledgeReportInterval,
        DateTime expirationDate)
    {
        return "KnowledgeReportInterval__1__23__2017-02-16";
    }

    public static bool IsValidUpdateCommand(string updateCommand, string token)
    {
        return true;
    }

    public static UIMessage Run(string updateCommand, string token)
    {
        var commandParts = Regex.Split(updateCommand, "__");
        UserSettingNotificationInterval knowledgeReportInterval = UserSettingNotificationInterval.NotSet;
        User user = null;
        DateTime expirationDate = new DateTime();

        return new SuccessMessage("not done yet here...");

        //try
        //{
        //    //throw if commandParts[0] != "KnowledgeReportInterval"
        //    knowledgeReportInterval = (UserSettingNotificationInterval)commandParts[1].ToInt();
        //    user = Sl.R<UserRepo>().GetById(commandParts[2].ToInt());
        //    expirationDate = DateTime.ParseExact(commandParts[3], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //    return new SuccessMessage("Hat geklappt! " + user.Name + " -- " + knowledgeReportInterval + " -- " + expirationDate);
        //}
        //catch (Exception exception)
        //{
        //    return new ErrorMessage(String.Join("----",commandParts) + " -.-.-.- " + exception.Message);
        //    //throw new Exception(exception.Message);
        //}
        //return new SuccessMessage("Hat geklappt! " + user.Name + " -- " + knowledgeReportInterval + " -- " + expirationDate);
    }
}
