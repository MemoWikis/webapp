using System.Collections.Generic;

public class TrainingSettingsDatesModel
{
    public IList<TrainingDateModel> Dates = new List<TrainingDateModel>();

    public TrainingSettingsDatesModel(Date date)
    {
        var futureTrainingDates = date.TrainingPlan.DatesInFuture;

        foreach (var trainingDate in futureTrainingDates)
        {
            Dates.Add(new TrainingDateModel
            {
                DateTime = trainingDate.DateTime,
                QuestionCount = trainingDate.AllQuestionsInTraining.Count,
                Date = date
            });
        }
    }
}