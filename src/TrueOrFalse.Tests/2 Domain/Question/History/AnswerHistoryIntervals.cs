using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using FluentNHibernate.Testing.Values;
using NHibernate.Mapping;
using NHibernate.Util;
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
        var answerRows = GetAnswerHistoryRows.Run(listOfAnswerHistories);
        var intervals1 = Intervalizer.Run(answerRows, new TimeSpan(1, 0, 0, 0));
        var intervals2 = Intervalizer.Run(new List<List<AnswerHistory>>(), new TimeSpan(1, 0, 0, 0));

        //ASSERT

    }
}

public class ExaminedAnswerObject
{
    public AnswerHistory ExaminedAnswer;
    public AnswerHistory PreviousAnswer;
    public List<AnswerHistory> AnswerHistoryRow;
    /// <summary>
    /// Time span between previous and examined answer
    /// </summary>
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public ExaminedAnswerObject(List<AnswerHistory> answerHistoryRow)
    {
        if (answerHistoryRow.Count <= 1) return;
        AnswerHistoryRow = answerHistoryRow;
        ExaminedAnswer = answerHistoryRow[answerHistoryRow.Count - 1];
        PreviousAnswer = answerHistoryRow[answerHistoryRow.Count - 2];
        TimePassed = ExaminedAnswer.DateCreated.Subtract(PreviousAnswer.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}

public class Intervalizer
{
    public static List<TimeIntervalWithAnswers> Run(List<List<AnswerHistory>> answerHistoryRows, TimeSpan intervalLength)
    {
        var listOfExaminedAnswerObjects = GetExaminedAnswerObjects.Run(answerHistoryRows).OrderBy(x => x.TimePassedInSeconds).ToList();
        
        var listOfIntervals = new List<TimeIntervalWithAnswers>();
        if (listOfExaminedAnswerObjects.Any())
        {
            var maxTimeSpanInSeconds = listOfExaminedAnswerObjects.Any() ? listOfExaminedAnswerObjects.Last().TimePassedInSeconds : 0;
            var numberOfIntervals = (int)Math.Floor(maxTimeSpanInSeconds / intervalLength.TotalSeconds) + 1;
            for (var i = 0; i < numberOfIntervals; i++)
            {
                listOfIntervals.Add(new TimeIntervalWithAnswers(intervalLength, i));
            }
            listOfExaminedAnswerObjects.ForEach(x =>
            {
                var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds / intervalLength.TotalSeconds);
                listOfIntervals[intervalIndex].AddPair(x.ExaminedAnswer, x.PreviousAnswer);
            });
        }

        return listOfIntervals;
    }
}

public class GetExaminedAnswerObjects
{
    public static List<ExaminedAnswerObject> Run(List<List<AnswerHistory>> answerHistoryRows)
    {
        var list = new List<ExaminedAnswerObject>();
        answerHistoryRows.ForEach(r => list.Add(new ExaminedAnswerObject(r)));

        return list;
    }
}

public class TimeIntervalWithAnswers
{
    public TimeSpan TimeIntervalLength;
    public int IndexOfInterval;
    public List<Tuple<AnswerHistory, AnswerHistory>> PairsOfPreviousAndExaminedAnswer = new List<Tuple<AnswerHistory, AnswerHistory>>();
    public int NumberOfPairs;
    public double ProportionAnsweredCorrect;
    public TimeSpan TimePassedLowerBound;
    public TimeSpan TimePassedUpperBound;

    public TimeIntervalWithAnswers(TimeSpan timeIntervalLength, int index)
    {
        TimeIntervalLength = timeIntervalLength;
        IndexOfInterval = index;
        TimePassedLowerBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds*index);
        TimePassedUpperBound = TimeSpan.FromSeconds(TimeIntervalLength.TotalSeconds*(index + 1) - 1);
        NumberOfPairs = 0;
    }

    public void AddPair(AnswerHistory examinedAnswer, AnswerHistory previousAnswer)
    {
        PairsOfPreviousAndExaminedAnswer.Add(new Tuple<AnswerHistory, AnswerHistory>(previousAnswer, examinedAnswer));
        NumberOfPairs = PairsOfPreviousAndExaminedAnswer.Count;
        ProportionAnsweredCorrect = PairsOfPreviousAndExaminedAnswer.Count(p => p.Item2.AnsweredCorrectly())/(double)NumberOfPairs;
    }
}

public class HelpingMethods
{
    
    public static List<Tuple<AnswerHistory, AnswerHistory>> GetConsecutivePairsFromAnswerHistoryRows(List<List<AnswerHistory>> answerHistoryRows)
    {
        var listOfPairs = new List<Tuple<AnswerHistory, AnswerHistory>>();

        answerHistoryRows.ForEach(r =>
        {
            for (var i = 0; i < r.Count - 1; i++)
            {
                listOfPairs.Add(new Tuple<AnswerHistory, AnswerHistory>(r[i], r[i + 1]));
            }
        });

        return listOfPairs;
    }

    public static List<Tuple<AnswerHistory, AnswerHistory>> GetLastPairsFromAnswerHistoryRows(List<List<AnswerHistory>> answerHistoryRows)
    {
        var listOfLastPairs = new List<Tuple<AnswerHistory, AnswerHistory>>();
        answerHistoryRows.ForEach(r =>
        {
            if (r.Count() > 1)
            {
                listOfLastPairs.Add(new Tuple<AnswerHistory, AnswerHistory>(r[r.Count() - 2], r[r.Count() - 1]));
            }
        });

        return listOfLastPairs;
    }
}




