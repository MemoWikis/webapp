using NUnit.Framework;
using TrueOrFalse.Tests;

public class Game_persistence : BaseTest
{
    [Test]
    public void Games_should_be_persisted()
    {
        var context = ContextGame.New();
        context.Add().Add().Persist();

        RecycleContainer();

        var allGames = R<GameRepo>().GetAll();
        Assert.That(allGames.Count, Is.EqualTo(2));
        Assert.That(allGames[0].Sets.Count, Is.EqualTo(1));
    }

    [Test]
    public void Should_detect_if_in_game()
    {
        var contextUser = ContextUser.New();
        contextUser.Add("user1").Add("user2").Add("user3").Persist();
        var user1 = contextUser.All[0];
        var user2 = contextUser.All[1];
        var user3 = contextUser.All[2];

        var contextGame = ContextGame.New();
        contextGame.Add().Add().Persist();

        var game1 = contextGame.All[0];
        var game2 = contextGame.All[1];
        game1.AddPlayer(user1);
        game1.AddPlayer(user2);
        game1.Status = GameStatus.InProgress;
        R<GameRepo>().Update(game1);
        
        RecycleContainer();

        var isInGame = Resolve<IsCurrentlyInGame>();

        Assert.That(isInGame.Yes(game1.Id, user1.Id), Is.True);
        Assert.That(isInGame.Yes(game1.Id, user2.Id), Is.True);
        Assert.That(isInGame.Yes(game1.Id, user3.Id), Is.False);
        Assert.That(isInGame.Yes(game2.Id, user1.Id), Is.False);
    }
}