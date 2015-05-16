using NUnit.Framework;
using TrueOrFalse.Tests;

public class Game_rounds : BaseTest
{
    [Test]
    public void Should_create_rounds()
    {
        var gameRepo = R<GameRepo>();
        var game = ContextGame.New().Add().Persist().All[0];
        var set = ContextSet.New()
            .AddSet("Set")
                .AddQuestion("A", "AS")
                .AddQuestion("B", "BS")
                .AddQuestion("C", "CS")
                .AddQuestion("B", "CS")
            .Persist()
            .All[0];

        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[0].Question });
        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[1].Question });
        game.AddRound(new GameRound { Set = set, Question = set.QuestionsInSet[2].Question });
        gameRepo.Update(game);
        gameRepo.Flush();
        
    }
}