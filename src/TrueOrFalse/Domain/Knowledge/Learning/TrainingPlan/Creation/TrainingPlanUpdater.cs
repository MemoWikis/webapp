using NHibernate;
using NHibernate.Util;

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

        var newTrainigPlan = TrainingPlanCreator.Run(trainingPlan.Date, trainingPlan.Settings);

        foreach (var newDate in newTrainigPlan.Dates)
            trainingPlan.Dates.Add(new TrainingDate
            {
                AllQuestions = newDate.AllQuestions,
                DateTime = newDate.DateTime,
            });

        trainingPlanRepo.Update(trainingPlan);
        trainingPlanRepo.Flush();
        return trainingPlan;
    }
}