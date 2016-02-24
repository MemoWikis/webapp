using System.IO;
using RazorEngine;

public class TrainingReminderMsg
{
    public static void SendHtmlMail(TrainingDate trainingDate)
    {
        var dateTitle = trainingDate.TrainingPlan.Date.GetTitle();

        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate_TrainingReminder()), 
            new TrainingReminderMsgModel
            {
                DateName = dateTitle,
                QuestionCount = trainingDate.AllQuestionsInTraining.Count.ToString(),
                TrainingLength = new TimeSpanLabel(trainingDate.TrainingPlan.TimeRemaining).Full
            }
        );

        HtmlMessage.Send(new MailMessage2(
            Settings.EmailFrom,
            trainingDate.UserEmail(),
            dateTitle,
            parsedTemplate)
        {UserName = trainingDate.User().Name});
    }
}