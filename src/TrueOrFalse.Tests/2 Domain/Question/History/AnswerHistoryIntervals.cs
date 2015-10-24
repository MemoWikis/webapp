using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class AnswerHistoryIntervals_test 
{
    [Test]
    public void Should_get_intervals()
    {
        //ARRANGE

        var listOfAnswerHistories = new List<AnswerHistory> { 
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 8, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 4, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 6, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 1, 12, 00, 00), AnswerredCorrectly = AnswerCorrectness.True},
		    new AnswerHistory {UserId = 1, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 01), AnswerredCorrectly = AnswerCorrectness.True},
		    
            new AnswerHistory {UserId = 1, QuestionId = 2, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    
            new AnswerHistory {UserId = 2, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new AnswerHistory {UserId = 2, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 01)},
		    
            new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 11, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 12, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 18, 12, 00, 00)},
		};
        //ACT
        var answerRows = AnswerPairFromHistoryRows.Get(listOfAnswerHistories);
        var intervals1 = Intervalizer.Run(answerRows, new TimeSpan(1, 0, 0, 0));
        var intervals2 = Intervalizer.Run(new List<AnswerPair>(), new TimeSpan(1, 0, 0, 0));

        //ASSERT

    }
}





