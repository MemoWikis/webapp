using System;
using NUnit.Framework;
using TrueOrFalse.Tests;

public class Game_rounds : BaseTest
{
    [Test]
    public void Should_persist_rounds()
    {
        var gameRepo = R<GameRepo>();
        var game = ContextGame.New().Add().Persist().All[0];
        var set = ContextSet.New()
            .AddSet("Set")
                .AddQuestion("A", "AS")
                .AddQuestion("B", "BS")
                .AddQuestion("C", "CS")
                .AddQuestion("D", "DS")
            .Persist()
            .All[0];

        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[0].Question });
        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[1].Question });
        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[2].Question });
        gameRepo.Update(game);
        gameRepo.Flush();
        
    }

    [Test]
    public void Should_create_random_rounds()
    {
        var set = ContextSet.New()
            .AddSet("Set")
                .AddQuestion("A", "AS")
                .AddQuestion("B", "BS")
                .AddQuestion("C", "CS")
                .AddQuestion("D", "DS")
            .Persist()
            .All[0];

        var game = ContextGame.New().Add().Persist().All[0];
        game.RoundCount = 50;
        game.Sets.Add(set);

        var firstItemIsACount = 0;
        for (var i = 0; i < 400; i++)
        {
            R<AddRoundsToGame>().Run(game);
            if (game.Rounds[i].Question.Text == "A")
                firstItemIsACount++;
        }

        Assert.That(firstItemIsACount > 85 && firstItemIsACount < 115, Is.True);
        Console.WriteLine(firstItemIsACount);
    }
}