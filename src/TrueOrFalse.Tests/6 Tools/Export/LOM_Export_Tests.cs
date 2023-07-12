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
        
            var category = ContextCategory.New().Add("Example category").Persist().All.First();
            var lomXmlCategory = category.ToLomXml(LifetimeScope.Resolve<CategoryRepository>());

            Console.Write(lomXmlCategory);

            var question = ContextQuestion.New(R<QuestionRepo>(),
                    R<AnswerRepo>(),
                    R<AnswerQuestion>(),
                    R<UserRepo>())
                .AddQuestion("Example question")
                .AddCategory("cat 1", R<EntityCacheInitializer>())
                .Persist()
                .All
                .First();
            var lomXmlQuestion = question.ToLomXml(LifetimeScope.Resolve<CategoryRepository>());

            Console.Write(lomXmlQuestion);
    }
}

