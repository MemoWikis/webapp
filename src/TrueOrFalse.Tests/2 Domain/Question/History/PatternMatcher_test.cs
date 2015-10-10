using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class PatternMatcher_test 
{
    [Test]
    public void Should_match_x_days_3()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count, Is.EqualTo(1));
        Assert.That(matches[0].Name, Is.EqualTo("X-Days-Exactly-3"));
    }

    [Test]
    public void Should_match_x_days_4()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count, Is.EqualTo(1));
        Assert.That(matches[0].Name, Is.EqualTo("X-Days-Exactly-4"));
    }

    [Test]
    public void Should_determine_max_streak()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 7, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 8, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 9, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 10, 14, 00, 21)},
        };

        //ACT
        var maxStreak = MaxStreak.MaxStreakDays(listOfAnswers);

        //ASSERT
        Assert.That(maxStreak, Is.EqualTo(5));
        Assert.That(MaxStreak.MaxStreakDays(new List<AnswerHistory>()), Is.EqualTo(0));
    }
}



public class AnswerPatternMatch
{
    public string Name;
}

public class AnswerPatternMatcher
{
    public static List<AnswerPatternMatch> Run(List<AnswerHistory> listOfAnswers)
    {
        var result = new List<AnswerPatternMatch>();


        AnswerPatternRepo.GetAll().ForEach(p => {
            if(p.IsMatch(listOfAnswers))
                result.Add(new AnswerPatternMatch { Name = p.Name });
        });

        return result;
    }
}

public class AnswerPatternRepo
{
    public static List<IAnswerPattern> GetAll()
    {
        return new List<IAnswerPattern>()
        {
            new XDaysExactly3(),
            new XDaysExactly4(),
        };
    }
}

public interface IAnswerPattern
{
    string Name { get; }
    bool IsMatch(List<AnswerHistory> listOfAnswers);
}

public class XDaysExactly
{
    public bool IsMatch(List<AnswerHistory> listOfAnswers, int days)
    {
        if (listOfAnswers.Count != days)
            return false;

        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();

        DateTime previousDate = DateTime.MinValue;
        for (var i = 0; i < listOfAnswersOrderedByDate.Count(); i++)
        {
            if (i == 0)
            {
                previousDate = listOfAnswersOrderedByDate[i].DateCreated;
                continue;
            }

            if (previousDate.AddDays(1).Date != listOfAnswersOrderedByDate[i].DateCreated.Date)
                return false;

            previousDate = listOfAnswersOrderedByDate[i].DateCreated;
        }

        return true;
    }
}

public class XDaysExactly3 : XDaysExactly, IAnswerPattern
{
    public string Name {get { return "X-Days-Exactly-3"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class XDaysExactly4 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-4"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}

public class MaxStreak
{
    public static int MaxStreakDays(List<AnswerHistory> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var count = listOfAnswersOrderedByDate.Count;
        var maxDays = 0;

        for (var i = 0; i < count; i++)
        {
            var maxDaysRange = 0;
            var range = listOfAnswersOrderedByDate.GetRange(i, count - i);
            var previousDate = DateTime.MinValue.Date;
            for (var j = 0; j < range.Count; j++)
            {
                if (j == 0)
                {
                    previousDate = range[j].DateCreated.Date;
                    maxDaysRange++;
                    continue;
                }

                if(range[j].DateCreated.Date == previousDate) continue;

                if (range.All(ah => ah.DateCreated.Date != previousDate.AddDays(1))) break;
                
                previousDate = range[j].DateCreated.Date;
                maxDaysRange ++;
            }

            if (maxDaysRange > maxDays)
            {
                maxDays = maxDaysRange;
            }
        }

        return maxDays;
    }
    public bool IsMatch(List<AnswerHistory> listOfAnswers, int days)
    {
        return false;
    }
}

public class MaxStreakOf3 : MaxStreak, IAnswerPattern
{
    
    public string Name { get { return "Max-Streak-Of-3"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return MaxStreakDays(listOfAnswers) == 3;
    }
}