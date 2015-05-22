public class GameInProgressPlayerModel : PlayBaseModel
{
    public int CurrentRoundNum;
    public int CurrentRoundLength;

    public Question Question;

    public GameInProgressPlayerModel(Game game) : base(game)
    {
        var currentRound = game.GetCurrentRound();
        CurrentRoundNum = game.GetCurrentRoundNumber();
        CurrentRoundLength = currentRound.RoundLength;
        Question = currentRound.Question;
    }
}