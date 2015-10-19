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











