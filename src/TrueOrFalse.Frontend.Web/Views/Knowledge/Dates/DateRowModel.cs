using System;
using System.Linq;

public class DateRowModel : BaseModel
{
    public Date Date;

    public int KnowledgeNotLearned;
    public int KnowledgeNeedsLearning;
    public int KnowledgeNeedsConsolidation;
    public int KnowledgeSolid;

    public int AmountQuestions;

    public bool ShowMinutesLeft;
    public bool ShowHoursLeft;
    
    public TimeSpanLabel RemainingLabel;

    public bool IsPast;
    public bool IsNetworkDate;

    public int CopiedCount;
    public User CopiedFromUser;
    public string CopiedFromUserName;

    public bool HideEditPlanButton;

    public int LearningSessionCount;
    public int LearningSessionQuestionsLearned;
    public TimeSpan TimeSpentLearning;
    public int TrainingDateCount;
    public string RemainingTrainingTime;

    public TrainingPlan TrainingPlan;

    public DateRowModel(Date date, bool isNetworkDate = false, bool hideEditPlanButton = false)
    {
        Date = date;

        var allQuestions = date.AllQuestions();
        AmountQuestions = allQuestions.Count;

        var summary = KnowledgeSummaryLoader.Run(UserId, allQuestions.GetIds(), onlyValuated: false);

        KnowledgeNotLearned = summary.NotLearned;
        KnowledgeNeedsLearning = summary.NeedsLearning;
        KnowledgeNeedsConsolidation = summary.NeedsConsolidation;
        KnowledgeSolid = summary.Solid;

        CopiedCount = date.CopiedInstances.Count;
        CopiedFromUserName = "";
        if (date.CopiedFrom != null)
        {
            CopiedFromUser = date.CopiedFrom.User;
            CopiedFromUserName = date.CopiedFrom.User.Name;
        }

        LearningSessionCount = date.LearningSessions.Count;
        LearningSessionQuestionsLearned = date.LearningSessionQuestionsAnswered();
        TimeSpentLearning = date.TimeSpentLearning();
        TrainingPlan = date.TrainingPlan ?? new TrainingPlan();
        TrainingDateCount = TrainingPlan.OpenDates.Count;
        RemainingTrainingTime = new TimeSpanLabel(TrainingPlan.TimeRemaining).Full;

        var remaining = date.Remaining();
        IsPast = remaining.TotalSeconds < 0;
        RemainingLabel = new TimeSpanLabel(remaining, IsPast);
        IsNetworkDate = isNetworkDate;
        HideEditPlanButton = hideEditPlanButton;
    }
}