using System.IO;
using System.Net.Mail;
using RazorEngine;

public class TrainingReminderMsg
{
    public static void Send(TrainingDate trainingDate)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_TrainingReminder()), 
            new TrainingReminderMsgModel
            {
                DateName = trainingDate.TrainingPlan.Date.GetTitle(),
                QuestionCount = trainingDate.AllQuestionsInTraining.Count.ToString(),
                TrainingLength = new TimeSpanLabel(trainingDate.TrainingPlan.TimeRemaining).Full
            }
        );

        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            trainingDate.UserEmail(),
            "Subject",
            parsedTemplate)
        {UserName = trainingDate.User().Name});
    }
}