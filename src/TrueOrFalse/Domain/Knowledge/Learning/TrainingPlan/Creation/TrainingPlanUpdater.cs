using System;

public class TrainingPlanUpdater
{
    public static TrainingPlan Run(int trainingPlanId)
    {
        return Run(Sl.Resolve<TrainingPlanRepo>().GetById(trainingPlanId));
    }

    public static TrainingPlan Run(TrainingPlan trainingPlan)
    {
        var trainingPlanRepo = Sl.R<TrainingPlanRepo>();
        trainingPlanRepo.DeleteDates(trainingPlan, DateTimeX.Now());

        var updatedTrainingPlan = TrainingPlanCreator.Run(trainingPlan.Date, trainingPlan.Settings);
        trainingPlanRepo.Update(updatedTrainingPlan);

        return updatedTrainingPlan;
    }
}