using System;

public class TrainingPlanUpdater
{
    public static TrainingPlan Run(int trainingPlanId)
    {
        return Run(Sl.Resolve<TrainingPlanRepo>().GetById(trainingPlanId));
    }

    public static TrainingPlan Run(TrainingPlan trainingPlan)
    {
        Sl.R<TrainingPlanRepo>().DeleteDates(trainingPlan, DateTimeX.Now());

        return TrainingPlanCreator.Run(trainingPlan.Date, trainingPlan.Settings);
    }
}