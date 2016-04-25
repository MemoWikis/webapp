using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class ContextTrainingPlan
{
    public List<TrainingPlan> All = new List<TrainingPlan>();
    public TrainingPlan Last() { return All.Last(); }

    public static ContextTrainingPlan New()
    {
        return new ContextTrainingPlan();
    }

    public ContextTrainingPlan Add(int numberOfQuestions, DateTime dateOfDate)
    {
        var contextSet = ContextSet.New().AddSet("Setname", numberOfQuestions: numberOfQuestions).Persist();
        var date = ContextDate.New().Add(contextSet.All, dateTime: dateOfDate).Persist().All[0];

        var trainingsPlan = TrainingPlanCreator.Run(
            date, new TrainingPlanSettings
            {
                QuestionsPerDate_IdealAmount = 5,
                QuestionsPerDate_Minimum = 3
            });

        All.Add(trainingsPlan);

        return this;
    }

    public ContextTrainingPlan Persist()
    {
        var repo = Sl.R<TrainingPlanRepo>();

        foreach(var trainingPlan in All)
            repo.Create(trainingPlan);

        repo.Flush();

        foreach (var trainingPlan in All)
            trainingPlan.Date.TrainingPlan = trainingPlan;

        repo.Flush();

        return this;
    }
}