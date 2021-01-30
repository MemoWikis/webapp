using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Crumbtrail_test : BaseTest
{
    [Test]
    public void Should_get_crumbtrail()
    {
        var context = ContextCategory.New();
        var rootElement = context.Add("1").Persist().All.First();

        var firstLevelChildren = context
            .Add("1.1", parent: rootElement)
            .Add("1.2", parent: rootElement)
            .Persist()
            .All;

        var secondLevelChildren = context
            .Add("1.1.1", parent: firstLevelChildren.ByName("1.1"))
            .Add("1.2.1", parent: firstLevelChildren.ByName("1.2"))
            .Add("1.2.2", parent: firstLevelChildren.ByName("1.2"))
            .Persist()
            .All;
        
        var thirdLevelChildren = context
            .Add("1.1.1.1", parent: secondLevelChildren.ByName("1.1.1"))
            .Add("1.2.1.1", parent: secondLevelChildren.ByName("1.2.1"))
            .Add("1.2.1.2", parent: secondLevelChildren.ByName("1.2.1"))
            .Add("1.2.2.1", parent: secondLevelChildren.ByName("1.2.2"))
            .Persist() 
            .All;


        var crumbtrail_1_1_1_1 = CrumbtrailService.Get(thirdLevelChildren.ByName("1.1.1.1"));
        Assert.That(crumbtrail_1_1_1_1.ToDebugString(), Is.EqualTo("1 => 1.1 => 1.1.1 => [1.1.1.1]"));

        var crumbtrail_1_2_2_1 = CrumbtrailService.Get(thirdLevelChildren.ByName("1.2.2.1"));
        Assert.That(crumbtrail_1_2_2_1.ToDebugString(), Is.EqualTo("1 => 1.2 => 1.2.2 => [1.2.2.1]"));
    }
}
