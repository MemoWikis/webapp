using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using NHibernate.Util;
using NUnit.Framework;

[TestFixture]
public class HistoryIntervals_test 
{
    [Test]
    public void Should_get_answer_rows()
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
		    new AnswerHistory {UserId = 2, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 1, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		    new AnswerHistory {UserId = 3, QuestionId = 2, DateCreated = new DateTime(2015, 10, 3, 12, 00, 00)},
		};
        //ACT
        var lists = GetAnswerRows.Run(listOfAnswerHistories);
        var pairs = GetPairsFromAnswerRows.Run(lists);
        var intervals = Intervalizer.Run(pairs, new TimeSpan(1, 0, 0, 0));

        //ASSERT

    }
}

public class GetAnswerRows
{
    public static List<List<AnswerHistory>> Run(List<AnswerHistory> listOfAnswerHistories)
    {
        var lists = new List<List<AnswerHistory>>();
            listOfAnswerHistories.GroupBy(ah => new {ah.UserId, ah.QuestionId})
                .ToList()
                .ForEach(g => lists.Add(g.OrderBy(ah => ah.DateCreated).ThenBy(ah => ah.Id).ToList()));
        return lists;
    }
}
 
public class GetPairsFromAnswerRows
{
    public static List<Tuple<AnswerHistory, AnswerHistory>> Run(List<List<AnswerHistory>> answerHistoryRows)
    {
        var listOfPairs = new List<Tuple<AnswerHistory, AnswerHistory>>();

        answerHistoryRows.ForEach(r =>
        {
            var count = r.Count;
            
            for (var i = 0; i < count - 1; i++)
            {
                listOfPairs.Add(new Tuple<AnswerHistory, AnswerHistory>(r[i], r[i+1]));
            }
        });

        return listOfPairs;
    }
}

public class GetPairInformationFromPairs
{
    public static List<HelpingClass> Run(List<Tuple<AnswerHistory, AnswerHistory>> listOfAnswerPairs)
    {
        var list = new List<HelpingClass>();
        listOfAnswerPairs.ForEach(p => list.Add(new HelpingClass(p)));

        return list;
    }
}

public class HelpingClass
{
    public Tuple<AnswerHistory, AnswerHistory> AnswerHistoryPair;
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public HelpingClass(Tuple<AnswerHistory, AnswerHistory> answerHistoryPair)
    {
        AnswerHistoryPair = answerHistoryPair;
        TimePassed = AnswerHistoryPair.Item2.DateCreated.Subtract(AnswerHistoryPair.Item1.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}

public class Intervalizer
{
    public static List<TimeIntervalWithAnswers> Run(List<Tuple<AnswerHistory, AnswerHistory>> listOfAnswerPairs, TimeSpan intervalLength)
    {
        var listOfHelpingClass = GetPairInformationFromPairs.Run(listOfAnswerPairs);
        var y = listOfHelpingClass.OrderBy(x => x.TimePassedInSeconds);
        var listOfIntervals = new List<TimeIntervalWithAnswers>();
        var maxTimeSpanInSeconds = y.Any() ? y.Last().TimePassedInSeconds : 0;
        var numberOfIntervals = (int)Math.Floor(maxTimeSpanInSeconds/intervalLength.TotalSeconds) + 1;
        for (int i = 0; i < numberOfIntervals; i++)
        {
            listOfIntervals.Add(new TimeIntervalWithAnswers(intervalLength, i));
        }
        listOfHelpingClass.ForEach(x =>
        {
            var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds/intervalLength.TotalSeconds);
            listOfIntervals[intervalIndex].AddPair(x.AnswerHistoryPair);
        });

        return listOfIntervals;
    } 
}

public class TimeIntervalWithAnswers
{
    public TimeSpan TimeInterval;
    public int IndexOfInterval;
    public List<Tuple<AnswerHistory, AnswerHistory>> AnswerHistoryPairs = new List<Tuple<AnswerHistory, AnswerHistory>>();
    public int NumberOfPairs;
    public double ShareAnsweredCorrect;
    public TimeSpan TimePassedLowerBound;
    public TimeSpan TimePassedUpperBound;

    public TimeIntervalWithAnswers(TimeSpan timeInterval, int index)
    {
        TimeInterval = timeInterval;
        IndexOfInterval = index;
        TimePassedLowerBound = TimeSpan.FromSeconds(TimeInterval.TotalSeconds*index);
        TimePassedUpperBound = TimeSpan.FromSeconds(TimeInterval.TotalSeconds*(index + 1) - 1);
        NumberOfPairs = 0;
    }

    public void AddPair(Tuple<AnswerHistory, AnswerHistory> answerHistoryPair)
    {
        AnswerHistoryPairs.Add(answerHistoryPair);
        NumberOfPairs = AnswerHistoryPairs.Count;
        ShareAnsweredCorrect = AnswerHistoryPairs.Count(p => p.Item2.AnsweredCorrectly())/(double)NumberOfPairs;
    }
}




