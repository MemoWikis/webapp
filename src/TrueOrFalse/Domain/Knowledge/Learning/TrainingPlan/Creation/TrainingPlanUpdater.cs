using System.Diagnostics;

public class TrainingPlanUpdater
{
    public static TrainingPlan Run(int trainingPlanId)
    {
        return Run(Sl.Resolve<TrainingPlanRepo>().GetById(trainingPlanId));
    }

    public static TrainingPlan Run(TrainingPlan trainingPlan, TrainingPlanSettings settings)
    {
        trainingPlan.Settings = settings;
        return Run(trainingPlan);
    }

    public static TrainingPlan Run(TrainingPlan trainingPlan)
    {
        var trainingPlanRepo = Sl.R<TrainingPlanRepo>();
        trainingPlanRepo.DeleteDatesAfter(trainingPlan, DateTimeX.Now());

        var newTrainingPlan = TrainingPlanCreator.Run(trainingPlan.Date, trainingPlan.Settings);

        var stopWatch = Stopwatch.StartNew();

        foreach (var newDate in newTrainingPlan.Dates)
            trainingPlan.Dates.Add(new TrainingDate
            {
                AllQuestions = newDate.AllQuestions,
                DateTime = newDate.DateTime,
            });

        trainingPlan.MarkDatesAsMissed();

        trainingPlanRepo.Update(trainingPlan);
        trainingPlanRepo.Flush();

        Logg.r().Information("Traingplan updated (in db): {duration}", stopWatch.Elapsed);

        return trainingPlan;
    }
}