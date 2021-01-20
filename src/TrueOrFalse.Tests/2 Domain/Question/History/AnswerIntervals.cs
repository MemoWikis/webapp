using System;
using System.Collections.Generic;
using NUnit.Framework;
using TrueOrFalse.Tests;

[TestFixture]
public class AnswerIntervals_test : BaseTest
{
    [Test]
    public void Should_get_intervals()
    {
        //ARRANGE
        var question1 = ContextQuestion.GetQuestion();
        var question2 = ContextQuestion.GetQuestion();

        var listOfAnswerHistories = new List<Answer> { 
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 8, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 4, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 6, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 1, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new Answer {UserId = 1, Question = question1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 01), AnswerredCorrectly = AnswerCorrectness.True},
		    
            new Answer {UserId = 1, Question = question2, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    
            new Answer {UserId = 2, Question = question1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new Answer {UserId = 2, Question = question1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 01)},
		    
            new Answer {UserId = 3, Question = question2, DateCreated = new DateTime(2015, 10, 11, 12, 00, 00)},
		    new Answer {UserId = 3, Question = question2, DateCreated = new DateTime(2015, 10, 12, 12, 00, 00)},
		    new Answer {UserId = 3, Question = question2, DateCreated = new DateTime(2015, 10, 18, 12, 00, 00)},
		};
        //ACT
        var answerRows = AnswerPairFromHistoryRows.Get(listOfAnswerHistories);
        var forgettingCurve1 = Intervalizer.Run(answerRows, new TimeSpan(1, 0, 0, 0));
        var forgettingCurve_noPairs = Intervalizer.Run(new List<AnswerPair>(), new TimeSpan(1, 0, 0, 0));
        var forgettingCurve_cappedIntervals = Intervalizer.Run(answerRows, new TimeSpan(1, 0, 0, 0), intervalCount:5);

        //ASSERT
        Assert.That(forgettingCurve1.TotalIntervals, Is.EqualTo(30));
        Assert.That(forgettingCurve1.TotalPairs, Is.EqualTo(8));
        Assert.That(forgettingCurve1.TimeSpanLength.Days, Is.EqualTo(1));

        Assert.That(forgettingCurve_noPairs.TotalIntervals, Is.EqualTo(30));
        
        Assert.That(forgettingCurve_cappedIntervals.TotalIntervals, Is.EqualTo(5));
        Assert.That(forgettingCurve1.TotalPairs, Is.EqualTo(8));
    }
}