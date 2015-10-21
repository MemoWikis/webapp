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
        var examinedAnswerObjects = GetExaminedAnswerObjects.Run(answerRows);
        var intervals1 = Intervalizer.Run(examinedAnswerObjects, new TimeSpan(1, 0, 0, 0));
        var intervals2 = Intervalizer.Run(new List<ExaminedAnswerObject>(), new TimeSpan(1, 0, 0, 0));

        //ASSERT

    }
}

public class ExaminedAnswerObject
{
    public AnswerHistory NextAnswer;
    public AnswerHistory ExaminedAnswer;
    public List<AnswerHistory> AnswerHistoryRow;
    /// <summary>
    /// Time span between examined and next answer
    /// </summary>
    public TimeSpan TimePassed;
    public double TimePassedInSeconds;

    public ExaminedAnswerObject(List<AnswerHistory> answerHistoryRow)
    {
        if (answerHistoryRow.Count <= 1) return;
        AnswerHistoryRow = answerHistoryRow;
        NextAnswer = answerHistoryRow[answerHistoryRow.Count - 1];
        ExaminedAnswer = answerHistoryRow[answerHistoryRow.Count - 2];
        TimePassed = NextAnswer.DateCreated.Subtract(ExaminedAnswer.DateCreated);
        TimePassedInSeconds = TimePassed.TotalSeconds;
    }
}

public class PrepareForgettingCurveData
{
    public static List<TimeIntervalWithAnswers> Run(List<AnswerHistory> answerHistories, TimeSpan intervalLength)
    {
        var answerHistoryRows = GetAnswerHistoryRows.Run(answerHistories);
        var listOfExaminedAnswerObjects = GetExaminedAnswerObjects.Run(answerHistoryRows).OrderBy(x => x.TimePassedInSeconds).ToList();
        return Intervalizer.Run(listOfExaminedAnswerObjects, intervalLength);
    }
}

public class Intervalizer
{
    public static List<TimeIntervalWithAnswers> Run(List<ExaminedAnswerObject> examinedAnswerObjects, TimeSpan intervalLength)
    {
        var listOfIntervals = new List<TimeIntervalWithAnswers>();
        if (examinedAnswerObjects.Any())
        {
            var maxTimeSpanInSeconds = examinedAnswerObjects.Any() ? examinedAnswerObjects.Last().TimePassedInSeconds : 0;
            var numberOfIntervals = (int)Math.Floor(maxTimeSpanInSeconds / intervalLength.TotalSeconds) + 1;
            for (var i = 0; i < numberOfIntervals; i++)
            {
                listOfIntervals.Add(new TimeIntervalWithAnswers(intervalLength, i));
            }
            examinedAnswerObjects.ForEach(x =>
            {
                var intervalIndex = (int)Math.Floor(x.TimePassedInSeconds / intervalLength.TotalSeconds);
                listOfIntervals[intervalIndex].AddPair(x.ExaminedAnswer, x.NextAnswer);
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
    public List<Tuple<AnswerHistory, AnswerHistory>> PairsOfExaminedAndNextAnswer = new List<Tuple<AnswerHistory, AnswerHistory>>();
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

    public void AddPair(AnswerHistory examinedAnswer, AnswerHistory nextAnswer)
    {
        PairsOfExaminedAndNextAnswer.Add(new Tuple<AnswerHistory, AnswerHistory>(examinedAnswer, nextAnswer));
        NumberOfPairs = PairsOfExaminedAndNextAnswer.Count;
        ProportionAnsweredCorrect = PairsOfExaminedAndNextAnswer.Count(p => p.Item2.AnsweredCorrectly())/(double)NumberOfPairs;
    }
}



