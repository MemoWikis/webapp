﻿using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

[Category(TestCategories.Integration)]
public class Should_recalculate_all_wish_knowledge_items : BaseTest
{
    [Test]
    public void Run()
    {
        var context = ContextQuestion.New(R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserWritingRepo>(), 
                R<CategoryRepository>())
            .AddQuestion(questionText: "1", solutionText: "")
            .AddQuestion(questionText: "2", solutionText: "")
            .AddQuestion(questionText: "3", solutionText: "")
            .Persist();

        var questIKnow = R<QuestionInKnowledge>();
        questIKnow.Create(new QuestionValuation { RelevancePersonal = 100, Question = context.All[0], User = context.Creator });
        questIKnow.Create(new QuestionValuation { RelevancePersonal = 1, Question = context.All[1], User = context.Creator });
        questIKnow.Create(new QuestionValuation { Question = context.All[2], User = context.Creator });

        Resolve<ISession>().Flush();

        Resolve<ProbabilityUpdate_ValuationAll>().Run();
        Assert.That(Resolve<GetWishQuestionCount>().Run(context.Creator.Id), Is.EqualTo(2));

        Resolve<ISession>().Flush();

        var summary = R<KnowledgeSummaryLoader>().Run(context.Creator.Id);
        Assert.That(summary.Total, Is.EqualTo(2));
        Assert.That(summary.NotLearned, Is.EqualTo(2));
    }
}