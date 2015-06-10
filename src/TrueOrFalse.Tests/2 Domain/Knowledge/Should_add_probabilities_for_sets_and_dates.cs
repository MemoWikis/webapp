using NUnit.Framework;
using TrueOrFalse.Tests;

public class Should_add_probabilities_for_sets_and_dates : BaseTest
{
    [Test]
    public void Sammple1_no_data()
    {
        var userContext = ContextUser.New().Add("Firstname Lastname").Persist();
        R<AddProbabilitiesEntries_ForSetsAndDates>().Run(userContext.All[0]);
    }

    [Test]
    public void Sample2_add_valuation_entries_for_dates()
    {
        var setContext = ContextSet.New()
            .AddSet("Set1", amountOfQuestions: 2)
            .AddSet("Set2", amountOfQuestions: 2)
            .AddSet("Set3", amountOfQuestions: 1)
            .Persist();

        var dateContext = ContextDate.New().Add(setContext.All).Persist();
        var user = dateContext.User;
        
        var valuationRepo = R<QuestionValuationRepo>();
        Assert.That(valuationRepo.GetByUser(dateContext.User).Count, Is.EqualTo(0));

        R<AddProbabilitiesEntries_ForSetsAndDates>().Run(user);
        Assert.That(valuationRepo.GetByUser(dateContext.User).Count, Is.EqualTo(5));

        var questionContext = ContextQuestion.New().AddQuestion().Persist();
        setContext
            .AddSet("Set4", questions: setContext.All[0].Questions())
            .AddSet("Set5", questions: questionContext.All)
            .Persist();

        R<AddProbabilitiesEntries_ForSetsAndDates>().Run(user);
        Assert.That(valuationRepo.GetByUser(dateContext.User).Count, Is.EqualTo(6));
    }
}