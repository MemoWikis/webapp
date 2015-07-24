using System;
using System.Linq;
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

        game.AddRound(new Round { Set = set, Question = set.QuestionsInSet[0].Question });
        game.AddRound(new Round { Set = set, Question = set.QuestionsInSet[1].Question });
        game.AddRound(new Round { Set = set, Question = set.QuestionsInSet[2].Question });
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

        var game = ContextGame.New().Add(amountQuestions:0).Persist().All[0];
        game.RoundCount = 50;
        game.Sets.Add(set);

        var firstItemIsA_Count = 0;
        for (var i = 0; i < 400; i++)
        {
            R<AddRoundsToGame>().Run(game);
            if (game.Rounds[1].Question.Text == "A") 
                firstItemIsA_Count++;
        }

        Assert.That(firstItemIsA_Count > 75 && firstItemIsA_Count < 125, Is.True, firstItemIsA_Count.ToString());
    }

    [Test]
    public void Should_not_create_more_rounds_then_questions()
    {
        var set = ContextSet.New()
            .AddSet("Set")
                .AddQuestion("A", "AS")
                .AddQuestion("B", "BS")
            .Persist()
            .All[0];

        var game = ContextGame.New().Add(amountQuestions: 0).Persist().All[0];
        game.RoundCount = 50;
        game.Sets.Add(set);

        R<AddRoundsToGame>().Run(game);

        Assert.That(game.RoundCount, Is.EqualTo(2));
        Assert.That(game.Rounds.Count, Is.EqualTo(2));


        //less rounds 
        game.RoundCount = 1;
        game.Sets.Add(set);

        R<AddRoundsToGame>().Run(game);
        Assert.That(game.RoundCount, Is.EqualTo(1));
        Assert.That(game.Rounds.Count, Is.EqualTo(1));

        //equal rounds 
        game.RoundCount = 2;
        game.Sets.Add(set);

        R<AddRoundsToGame>().Run(game);
        Assert.That(game.RoundCount, Is.EqualTo(2));
        Assert.That(game.Rounds.Count, Is.EqualTo(2));
    }

    [Test]
    public void Should_progress_rounds()
    {
        var game = ContextGame.New().Add(amountQuestions:4).Persist().All[0];
        
        Assert.That(game.Rounds.All(r => r.Status != GameRoundStatus.Completed), Is.True);
        game.NextRound();
        Assert.That(game.Rounds[0].Status == GameRoundStatus.Current, Is.True);
        Assert.That(game.Rounds.Count(x => x.Status == GameRoundStatus.Current), Is.EqualTo(1));
        game.NextRound();
        Assert.That(game.Rounds[1].Status == GameRoundStatus.Current, Is.True);
        Assert.That(game.Rounds.Count(x => x.Status == GameRoundStatus.Current), Is.EqualTo(1));
        game.NextRound();
        Assert.That(game.Rounds[2].Status == GameRoundStatus.Current, Is.True);
        game.NextRound();
        Assert.That(game.IsLastRoundCompleted(), Is.False);
        Assert.That(game.Rounds[3].Status == GameRoundStatus.Current, Is.True);
        Assert.That(game.IsLastRoundCompleted(), Is.False);
        game.NextRound();
        Assert.That(game.Rounds[3].Status == GameRoundStatus.Current, Is.False);
        Assert.That(game.Rounds.Count(x => x.Status == GameRoundStatus.Current), Is.EqualTo(0));
        Assert.That(game.IsLastRoundCompleted(), Is.True);
    }
}