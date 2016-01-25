using System;
using System.Collections.Generic;
using System.Linq;

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
        var contextDate = ContextDate.New().Add(contextSet.All, dateTime: dateOfDate).Persist();

        var trainingsPlan = TrainingPlanCreator.Run(
            contextDate.All[0], new TrainingPlanSettings
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

        foreach(var trainingsPlan in All)
            repo.Create(trainingsPlan); 
 
        return this;
    }
}