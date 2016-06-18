using System;

public class TrainingDateModel
{
    public int Id;
    public DateTime DateTime;
    public int QuestionCount;

    public Date Date;

    public int LearningTimeInMin => (int) Math.Ceiling(30/*seconds per question*/ * QuestionCount / 60d);

    private TimeSpanLabel _timeSpanLabel;
    public TimeSpanLabel TimeSpanLabel => _timeSpanLabel ?? (_timeSpanLabel = new TimeSpanLabel(DateTime.Now - DateTime));

    public bool IsContinousLearning => Date == null;

    public KnowledgeSummary SummaryBefore;
    public KnowledgeSummary SummaryAfter;

    public TrainingDateModel() {}
    public TrainingDateModel(TrainingDate trainingDate)
    {
        Id = trainingDate.Id;
        DateTime = trainingDate.DateTime;
        QuestionCount = trainingDate.AllQuestionsInTraining.Count;
        Date = trainingDate.TrainingPlan.Date;
        SummaryBefore = trainingDate.GetSummaryBefore();
        SummaryAfter = trainingDate.GetSummaryAfter();
    }

    public string GetTitle()
    {
        return Date != null ?
            Date.GetTitle() :
            "Langzeitlernen";
    }
}