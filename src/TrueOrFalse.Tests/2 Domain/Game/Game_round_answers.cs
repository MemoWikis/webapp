using NUnit.Framework;

public class Game_round_answers : BaseTest
{
    [Test]
    public void Should_persist_answers()
    {
        var game = ContextGame.New().Add(
            amountQuestions: 10, 
            amountPlayers: 2).Persist().All[0];

        var player1 = game.Players[0];
        var player2 = game.Players[1];

        for (var i = 0; i < 10; i++)
        {
            var round = game.Rounds[i];
            //player 1: always wrong
            round.Answers.Add(new AnswerHistory{
                AnswerredCorrectly = AnswerCorrectness.False,
                QuestionId = round.Question.Id,
                AnswerText = "Foo",
                Round = round,
                UserId = player1.User.Id,
                Player = player1
            });
            //player 2: always correct
            round.Answers.Add(new AnswerHistory{
                AnswerredCorrectly = AnswerCorrectness.True,
                QuestionId = round.Question.Id,
                AnswerText = round.Question.Text,
                Round = round,
                UserId = player2.User.Id,
                Player = player2
            });
        }

        R<GameRepo>().Update(game);

        RecycleContainer();

        var gameFromDb = R<GameRepo>().GetById(game.Id);
        Assert.That(gameFromDb.Rounds[0].Answers.Count, Is.EqualTo(2));
        Assert.That(gameFromDb.Rounds[9].Answers.Count, Is.EqualTo(2));
        Assert.That(gameFromDb.Players[0].Answers.Count, Is.EqualTo(10));
        Assert.That(gameFromDb.Players[1].Answers.Count, Is.EqualTo(10));
    }
}