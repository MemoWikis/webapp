using System;
using System.Diagnostics;

public class TrainingPlanUpdater
{
    public static void Run(TrainingPlan trainingPlan, TrainingPlanSettings settings)
    {
        trainingPlan.Settings = settings;
        Run(trainingPlan);
    }

    public static void Run(TrainingPlan trainingPlan)
    {
        var trainingPlanRepo = Sl.R<TrainingPlanRepo>();
        trainingPlanRepo.DeleteUnstartedDatesAfter(trainingPlan, DateTimeX.Now());

        var newTrainingPlan = TrainingPlanCreator.Run(trainingPlan.Date, trainingPlan.Settings);

        var stopWatch = Stopwatch.StartNew();

        foreach (var newDate in newTrainingPlan.Dates)
            trainingPlan.Dates.Add(new TrainingDate
            {
                AllQuestions = newDate.AllQuestions,
                DateTime = newDate.DateTime,
                IsBoostingDate = newDate.IsBoostingDate,
                ExpiresAt = newDate.ExpiresAt
            });

        trainingPlan.LearningGoalIsReached = newTrainingPlan.LearningGoalIsReached;

        trainingPlan.MarkDatesAsMissed();
        trainingPlan.CompleteUnfinishedSessions();

        trainingPlanRepo.Update(trainingPlan);
        trainingPlanRepo.Flush();

        Logg.r().Information("Traingplan updated (in db): {duration}", stopWatch.Elapsed);
    }
}