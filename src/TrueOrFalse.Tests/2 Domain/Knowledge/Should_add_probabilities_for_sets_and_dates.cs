using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_add_probabilities_for_sets_and_dates : BaseTest
{
    [Test]
    public void Sammple1_no_data()
    {
        var userContext = ContextUser.New().Add("Firstname Lastname").Persist();
        R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(userContext.All[0]);
    }

    [Test]
    public void Sample2_add_valuation_entries_for_dates()
    {
        var setContext = ContextSet.New()
            .AddSet("Set1", numberOfQuestions: 2)
            .AddSet("Set2", numberOfQuestions: 2)
            .AddSet("Set3", numberOfQuestions: 1)
            .Persist();

        var dateContext = ContextDate.New().Add(setContext.All).Persist();
        var user = dateContext.User;
        
        var valuationRepo = R<QuestionValuationRepo>();
        Assert.That(valuationRepo.GetByUser(user).Count, Is.EqualTo(0));

        R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(user);
        Assert.That(valuationRepo.GetByUser(user, onlyActiveKnowledge:false).Count, Is.EqualTo(5));

        var questionContext = ContextQuestion.New().AddQuestion().Persist();
        ContextSet.New()
            .AddSet("Set4", questions: setContext.All[0].Questions(), creator: user)
            .AddSet("Set5", questions: questionContext.All, creator: user)
            .AddSet("Set6", numberOfQuestions: 5, creator: user)
            .Persist();

        R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(user);
        Assert.That(valuationRepo.GetByUser(dateContext.User, onlyActiveKnowledge:false).Count, Is.EqualTo(5));
    }

}