using System;

public class GameInProgressPlayerModel : PlayBaseModel
{
    public int RoundNum;
    public int RoundLength;
    public DateTime RoundEndTime;
    public Question Question;

    public GameInProgressPlayerModel(Game game) : base(game)
    {
        var currentRound = game.GetCurrentRound();
        RoundNum = game.GetCurrentRoundNumber();
        RoundLength = currentRound.RoundLength;
        RoundEndTime = currentRound.StartTime.Value.AddSeconds(currentRound.RoundLength);
        Question = currentRound.Question;
    }
}