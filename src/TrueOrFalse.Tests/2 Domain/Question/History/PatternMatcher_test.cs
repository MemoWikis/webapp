using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class PatternMatcher_test 
{
    [Test]
    public void Should_match_pattern_XDaysExactly3()
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
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly), Is.EqualTo(1));
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly3), Is.EqualTo(1));
    }

    [Test]
    public void Should_match_pattern_XDaysExactly4()
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
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly), Is.EqualTo(1));
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly4), Is.EqualTo(1));
    }

    [Test]
    public void Should_match_pattern_XDaysExactly6OrMore()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 1, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 2, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 7, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly), Is.EqualTo(1));
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly6OrMore), Is.EqualTo(1));
    }

    [Test]
    public void Should_not_match_pattern_XDaysExactly()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count(m => m.Pattern is XDaysExactly), Is.EqualTo(0));
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
        var maxStreak = MaxStreak.GetNumber(listOfAnswers);

        //ASSERT
        Assert.That(maxStreak, Is.EqualTo(5));
        Assert.That(MaxStreak.GetNumber(new List<AnswerHistory>()), Is.EqualTo(0));
    }

    [Test]
    public void Should_match_pattern_MaxStreakOf4()
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
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count(m => m.Pattern is MaxStreak), Is.EqualTo(1));
        Assert.That(matches.Count(m => m.Pattern is MaxStreakOf4), Is.EqualTo(1));
    }

    [Test]
    public void Should_match_pattern_MaxStreakOfMoreThan5()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 7, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 8, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 9, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count(m => m.Pattern is MaxStreak), Is.EqualTo(1));
        Assert.That(matches.Count(m => m.Pattern is MaxStreakOfMoreThan5), Is.EqualTo(1));
    }

    [Test]
    public void Should_not_match_pattern_MaxStreak()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 8, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count(m => m.Pattern is MaxStreak), Is.EqualTo(0));
    }
}

public class AnswerPatternMatch
{
    public string Name;
    public Type Type;
    public IAnswerPattern Pattern;
}

public class AnswerPatternMatcher
{
    public static List<AnswerPatternMatch> Run(List<AnswerHistory> listOfAnswers)
    {
        var result = new List<AnswerPatternMatch>();

        AnswerPatternRepo.GetAll().ForEach(p => {
            if(p.IsMatch(listOfAnswers))
                result.Add(new AnswerPatternMatch
                {
                    Name = p.Name, 
                    Type = p.GetType(), 
                    Pattern = p
                });
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
            new XDaysExactly2(),
            new XDaysExactly3(),
            new XDaysExactly4(),
            new XDaysExactly5(),
            new XDaysExactly6OrMore(),

            new MaxStreakOf2(),
            new MaxStreakOf3(),
            new MaxStreakOf4(),
            new MaxStreakOf5(),
            new MaxStreakOfMoreThan5()
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

        return GetNumber(listOfAnswers) == days;
    }

    public static int GetNumber(List<AnswerHistory> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var numberOfDays = 0;

        var previousDate = DateTime.MinValue;
        for (var i = 0; i < listOfAnswersOrderedByDate.Count(); i++)
        {
            if (i == 0)
            {
                previousDate = listOfAnswersOrderedByDate[i].DateCreated;
                numberOfDays++;
                continue;
            }

            if (previousDate.AddDays(1).Date != listOfAnswersOrderedByDate[i].DateCreated.Date)
                break;

            previousDate = listOfAnswersOrderedByDate[i].DateCreated;
            numberOfDays++;
        }

        return numberOfDays;
    }
}

public class XDaysExactly2 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-2"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 2);
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

public class XDaysExactly5 : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-5"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 5);
    }
}

public class XDaysExactly6OrMore : XDaysExactly, IAnswerPattern
{
    public string Name { get { return "X-Days-Exactly-6-Or-More"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return GetNumber(listOfAnswers) > 5;
    }
}

public class MaxStreak
{
    public static int GetNumber(List<AnswerHistory> listOfAnswers)
    {
        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();
        var count = listOfAnswersOrderedByDate.Count;
        var maxStreak = 0;

        for (var i = 0; i < count; i++)
        {
            var maxStreakInRange = 0;
            var range = listOfAnswersOrderedByDate.GetRange(i, count - i);
            var previousDate = DateTime.MinValue.Date;
            for (var j = 0; j < range.Count; j++)
            {
                if (j == 0)
                {
                    previousDate = range[j].DateCreated.Date;
                    maxStreakInRange++;
                    continue;
                }

                if(range[j].DateCreated.Date == previousDate) continue;

                if (range.All(ah => ah.DateCreated.Date != previousDate.AddDays(1))) break;
                
                previousDate = range[j].DateCreated.Date;
                maxStreakInRange++;
            }

            if (maxStreakInRange > maxStreak)
            {
                maxStreak = maxStreakInRange;
            }
        }

        return maxStreak;
    }
    public bool IsMatch(List<AnswerHistory> listOfAnswers, int days)
    {
        return GetNumber(listOfAnswers) == days;
    }
}

public class MaxStreakOf2 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-2"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 2);
    }
}

public class MaxStreakOf3 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-3"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class MaxStreakOf4 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-4"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}

public class MaxStreakOf5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-5"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 5);
    }
}

public class MaxStreakOfMoreThan5 : MaxStreak, IAnswerPattern
{
    public string Name { get { return "Max-Streak-Of-More-Than-5"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return GetNumber(listOfAnswers) > 5;
    }
}

