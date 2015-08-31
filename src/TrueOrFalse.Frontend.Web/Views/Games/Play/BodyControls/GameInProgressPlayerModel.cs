using System;

public class GameInProgressPlayerModel : PlayBaseModel
{
    public int RoundNum;
    public int RoundLength;
    public DateTime RoundEndTime;
    
    public Question Question;
    public Game Game;
    public Round Round;

    public Player Player;
    public User Watcher;
    public bool IsWatcher;

    public GameInProgressPlayerModel(Game game) : base(game)
    {
        var currentRound = game.GetCurrentRound();
        RoundNum = game.GetCurrentRoundNumber();
        RoundLength = currentRound.RoundLength;
        RoundEndTime = currentRound.StartTime.Value.AddSeconds(currentRound.RoundLength);
        
        Question = currentRound.Question;
        Game = game;
        Round = currentRound;
        Player = game.Players.ByUserId(UserId);

        if (Player == null)
        {
            IsWatcher = true;
            Watcher = _sessionUser.IsLoggedIn ? _sessionUser.User : new User();
            Player = new Player();
        }

    }
}