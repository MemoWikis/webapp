using System.Collections.Generic;
using System.Linq;

public class GameCompletedModel : PlayBaseModel
{
    public bool IsPlayer;
    public int PlayerPosition = -1;

    public IList<PlayerResultRow> Rows = new List<PlayerResultRow>();

    public GameCompletedModel(Game game) : base(game)
    {
        game.SetPlayerPositions();

        if (game.Players.Any(p => p.User.Id == UserId))
        {
            var thisPlayer = game.Players.First(p => p.User.Id == UserId);
            
            IsPlayer = true;
            PlayerPosition = thisPlayer.Position;
        }

        foreach (var player in game.Players)
        {
            var totalCorrect = player.Answers.Count(x => x.AnswerredCorrectly == AnswerCorrectness.True);
            var totalWrong = player.Answers.Count(x => x.AnswerredCorrectly == AnswerCorrectness.False);

            Rows.Add(new PlayerResultRow
            {
                Position = player.Position,
                IsCurrentUser = UserId == player.User.Id,
                PlayerName = player.User.Name,
                TotalQuestions = game.Rounds.Count,
                TotalCorrect = totalCorrect,
                TotalWrong = totalWrong,
                TotalNotAnswered = game.Rounds.Count - totalCorrect - totalWrong
            });
        }
    }
}

public class PlayerResultRow
{
    public int Position;
    public bool IsCurrentUser;
    public string PlayerName;

    public int TotalQuestions;
    public int TotalCorrect;
    public int TotalCorrectPercentage;
    public int TotalWrong;
    public int TotalWrongPercentage;
    public int TotalNotAnswered;
    public int TotalNotAnsweredPercentage;
}