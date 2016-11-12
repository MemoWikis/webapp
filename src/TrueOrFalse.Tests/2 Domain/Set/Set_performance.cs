using System;
using System.Linq;
using NHibernate;
using NUnit.Framework;

public class Set_performance : BaseTest
{
    [Test]
    public void Ensure_that_only_one_query_is_performed_when_loading_set()
    {
        var set = ContextSet.New()
            .AddSet("First set", numberOfQuestions: 10, amountCategoriesPerQuestion: 3)
            .AddSet("Seconds")
            .Persist().All
            .First();

        RecycleContainer();

        var session = R<ISession>();
        var setRepo = Resolve<SetRepo>();

        var setFromDb = setRepo.GetByIdEager(set.Id);

        Assert.That(session.SessionFactory.Statistics.IsStatisticsEnabled);
        Assert.That(session.SessionFactory.Statistics.PrepareStatementCount, Is.EqualTo(1));

        Assert.That(set.Questions().Count, Is.EqualTo(10));

        //access all questions and categories
        foreach (var question in setFromDb.Questions())
        {
            Console.WriteLine(question.Creator);
            Console.WriteLine(question.Categories.Count);
        }

        Assert.That(session.SessionFactory.Statistics.PrepareStatementCount, Is.EqualTo(3));

        Console.WriteLine(set.Questions().GetAllCategories().Select(c => c.Id));
        Console.WriteLine(set.Categories.Count);

        Assert.That(session.SessionFactory.Statistics.PrepareStatementCount, Is.EqualTo(3));
    }
}