using NUnit.Framework;

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
}