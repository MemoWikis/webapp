using System;
using System.Linq;
using NHibernate;
using NUnit.Framework;

public class Set_benchmark : BaseTest
{
    [Test]
    public void Ensure_that_only_one_query_is_performed_when_loading_set()
    {
        var set = ContextSet.New()
            .AddSet("First set", numberOfQuestions: 10, amountCategoriesPerQuestion: 3)
            .Persist().All
            .First();

        RecycleContainer();

        var session = R<ISession>();
        var setRepo = Resolve<SetRepo>();

        var setFromDb = setRepo.GetById(set.Id);

        Assert.That(session.SessionFactory.Statistics.IsStatisticsEnabled);

        //access all questions and categories
        foreach (var question in setFromDb.Questions())
        {
            Console.WriteLine(question.Creator);
            Console.WriteLine(question.Categories.Count);
        }

        Assert.That(session.SessionFactory.Statistics.QueryExecutionCount, Is.EqualTo(1));
    }
}