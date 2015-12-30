using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

class TrainingPlan_persistence : BaseTest
{
    [Test]
    public void Should_persist()
    {
        var contextSet = ContextSet.New().AddSet("Setname", numberOfQuestions: 10).Persist();
        var contextDate = ContextDate.New().Add(contextSet.All).Persist();

        var training = new TrainingPlan();
        training.Date = contextDate.All.First();
        training.Dates = new List<TrainingDate>();
        training.Dates.Add(new TrainingDate
        {
            AllQuestions = contextSet.All[0]
                .Questions().Take(5)
                .Select(x => new TrainingQuestion {Question = x}).ToList(),
            DateTime = DateTime.Now.AddDays(3)
        });
        training.Dates.Add(new TrainingDate
        {
            AllQuestions = contextSet.All[0]
                .Questions().Skip(5).Take(3)
                .Select(x => new TrainingQuestion {Question = x}).ToList(),
            DateTime = DateTime.Now.AddDays(5)
        });

        Sl.R<TrainingPlanRepo>().Create(training);

        RecycleContainer();

        var allTrainingPlans = Sl.R<TrainingPlanRepo>().GetAll();

        Assert.That(allTrainingPlans.Count, Is.EqualTo(1));
        Assert.That(allTrainingPlans[0].Questions.Count, Is.EqualTo(8));
        Assert.That(allTrainingPlans[0].Dates.Count, Is.EqualTo(2));

        var dates = allTrainingPlans[0].Dates.OrderBy(x => x.DateTime).ToList();
        Assert.That(dates[0].AllQuestions.Count, Is.EqualTo(5));
        Assert.That(dates[1].AllQuestions.Count, Is.EqualTo(3));
    }
}