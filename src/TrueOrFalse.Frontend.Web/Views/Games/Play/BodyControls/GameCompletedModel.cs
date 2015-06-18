using System.Collections.Generic;

public class GameCompletedModel : PlayBaseModel
{
    public IList<PlayerResultRow> Rows = new List<PlayerResultRow>();

    public GameCompletedModel(Game game) : base(game)
    {
        foreach (var player in game.Players)
        {
            Rows.Add(new PlayerResultRow
            {
                Position = player.Position,
                IsCurrentUser = UserId == player.User.Id,
                PlayerName = player.User.Name,
                TotalQuestions = game.Rounds.Count,
                TotalCorrect = player.AnsweredCorrectly,
                TotalWrong = player.AnsweredWrong,
                TotalNotAnswered = game.Rounds.Count - player.AnsweredCorrectly - player.AnsweredWrong
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