using NUnit.Framework;

public class Game_round_answers : BaseTest
{
    [Test]
    public void Should_persist_answers()
    {
        var game = ContextGame.New().Add(
            amountQuestions: 10, 
            amountPlayers: 2).Persist().All[0];
        
        for (var i = 0; i < 10; i++)
        {
            var round = game.Rounds[i];
            //player 1: always wrong
            round.Answers.Add(new AnswerHistory{
                AnswerredCorrectly = AnswerCorrectness.False,
                AnswerText = "Foo",
                Round = round,
            });
            //player 2: always correct
            round.Answers.Add(new AnswerHistory{
                AnswerredCorrectly = AnswerCorrectness.True,
                AnswerText = round.Question.Text,
                Round = round
            });
        }

        R<GameRepo>().Update(game);

        RecycleContainer();

        var gameFromDb = R<GameRepo>().GetById(game.Id);
        Assert.That(gameFromDb.Rounds[0].Answers.Count, Is.EqualTo(2));
        Assert.That(gameFromDb.Rounds[9].Answers.Count, Is.EqualTo(2));
        Assert.That(gameFromDb.Players[0].Answers.Count, Is.EqualTo(9));
    }
}