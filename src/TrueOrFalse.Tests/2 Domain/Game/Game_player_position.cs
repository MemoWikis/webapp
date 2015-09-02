using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class Game_player_position
{
    [Test]
    public void Should_correctly_set_position()
    {
        var game = new Game();
        game.Players = new List<Player>
        {
            new Player
            {
                Id = 1,
                Answers = new List<AnswerHistory> {
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.True},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False}
                }
            },
            new Player
            {
                Id = 2,
                Answers = new List<AnswerHistory> {
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.True},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False}
                }
            },
            new Player
            {
                Id = 3,
                Answers = new List<AnswerHistory> {
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.True},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False}
                }
            },
            new Player
            {
                Id = 4,
                Answers = new List<AnswerHistory> {
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.True},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False},
                    new AnswerHistory {AnswerredCorrectly = AnswerCorrectness.False}
                }
            },
        };

        game.SetPlayerPositions();

        Assert.That(game.Players.First(p => p.Id == 1).Position, Is.EqualTo(2));
        Assert.That(game.Players.First(p => p.Id == 2).Position, Is.EqualTo(1));
        Assert.That(game.Players.First(p => p.Id == 3).Position, Is.EqualTo(1));
        Assert.That(game.Players.First(p => p.Id == 4).Position, Is.EqualTo(2));
    }
}
