using System.IO;
using System.Net.Mail;
using RazorEngine;

public class TrainingReminderMsg
{
    public static void Send(TrainingDate trainingDate)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_TrainingReminder()), 
            new TrainingReminderMsgModel {Value1 = "Value1", Value2 = "Value2"}
        );

        SendEmail.Run(new MailMessage(
            Settings.EmailFrom,
            trainingDate.TrainingPlan.Date.User.EmailAddress,
            "Subject",
            parsedTemplate){
            IsBodyHtml = true
        });
    }
}