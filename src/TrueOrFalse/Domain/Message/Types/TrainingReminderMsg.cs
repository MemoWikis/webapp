using System.IO;
using RazorEngine;

public class TrainingReminderMsg
{
    public const string UtmSource = "trainingReminderDate";

    public const string SignOutMessage = "Du erhälst diese E-Mail, weil du einen Termin erstellt hast. " +
                                         "Wenn du diese Lernerinnerungen nicht mehr erhalten möchtest, " +
                                         "deaktiviere die E-Mail-Benachrichtigung bei den " +
                                         "<a href=\"https://memucho.de/Termine\">Einstellungen zu diesem Termin</a>.";

    public static void SendHtmlMail(TrainingDate trainingDate)
    {
        var dateTitle = trainingDate.TrainingPlan.Date.GetTitle();

        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_TrainingReminder()), 
            new TrainingReminderMsgModel(trainingDate)
        );

        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            trainingDate.UserEmail(),
            "Lerne jetzt für deinen Termin \"" + dateTitle + "\"",
            parsedTemplate)
        {UserName = trainingDate.User().Name},
        signOutMessage: SignOutMessage,
        utmSource: UtmSource);
    }
}