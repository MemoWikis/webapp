using System;
using System.Linq;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class LOM_Export_Tests : BaseTest
{
    [Test]
    public void Should_export_LOM()
    {
        using (var scope = _container.BeginLifetimeScope())
        {
            var category = ContextCategory.New().Add("Example category").Persist().All.First();
            var lomXmlCategory = category.ToLomXml(scope.Resolve<CategoryRepository>());

            Console.Write(lomXmlCategory);

            var question = ContextQuestion.New().AddQuestion("Example question").AddCategory("cat 1").Persist().All
                .First();
            var lomXmlQuestion = question.ToLomXml(scope.Resolve<CategoryRepository>());

            Console.Write(lomXmlQuestion);
        }
    }
}

