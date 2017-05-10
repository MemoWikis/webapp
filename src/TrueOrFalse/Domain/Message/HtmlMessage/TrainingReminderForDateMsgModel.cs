using System.Linq;
using Seedworks.Web.State;
using TrueOrFalse.Frontend.Web.Code;

public class TrainingReminderForDateMsgModel
{
    public string DateName;
    public string DateOfDate;
    public string DateAsDistance;
    public string QuestionCountTrainingSession;
    public string TrainingLengthTrainingSession;
    public string LinkToLearningSession;

    public string LearningContentFullString;
    public string RemainingTrainingSessionTime;
    public string RemainingTrainingSessionCount;

    public string LinkToDates;
    public string LinkToTechInfo;

    public string UtmSourceFullString;
    public string UtmCampaignFullString = "";

    public TrainingReminderForDateMsgModel(TrainingDate trainingDate, string utmSource = "trainingReminderDate")
    {
        UtmSourceFullString = string.IsNullOrEmpty(utmSource) ? "" : "&utm_source=" + utmSource;

        var date = trainingDate.TrainingPlan.Date;
        DateName = date.GetTitle();
        DateOfDate = date.DateTime.ToString("'am' dd.MM.yyyy 'um' HH:mm");
        var remainingLabel = new TimeSpanLabel(date.Remaining(), useDativ: true);
        DateAsDistance = remainingLabel.Full;
        QuestionCountTrainingSession = trainingDate.AllQuestionsInTraining.Count.ToString();
        TrainingLengthTrainingSession = new TimeSpanLabel(trainingDate.TimeEstimated()).Full;

        LearningContentFullString = date.CountQuestions().ToString() + " Frage" + StringUtils.PluralSuffix(date.CountQuestions(),"n");
        if (date.Sets.Any())
        {
            LearningContentFullString += " aus " + date.Sets.Count.ToString() + " Lernset" +
                                         StringUtils.PluralSuffix(date.Sets.Count, "s");
        }

        RemainingTrainingSessionTime = new TimeSpanLabel(trainingDate.TrainingPlan.TimeRemaining).Full;
        RemainingTrainingSessionCount = trainingDate.TrainingPlan.OpenDates.Count.ToString();


        /* Create Links */

        LinkToLearningSession = "https://memucho.de/Termin/Lernen/" + date.Id;
        LinkToDates = "https://memucho.de/Termine";
        LinkToTechInfo = "https://memucho.de/AlgoInsight/Forecast";
    }
}