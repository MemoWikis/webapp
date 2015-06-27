using System;

public class GameStatusChange : IRegisterAsInstancePerLifetime
{
    public void Cancel(int gameId)
    {
        var gameRepo = Sl.R<GameRepo>();
        var game = gameRepo.GetById(gameId);
        game.Status = GameStatus.Canceled;
        gameRepo.Update(game);
        gameRepo.Flush();

        Sl.R<GameHubConnection>().SendCancelation(gameId);
    }

    public void Start(int gameId)
    {
        var gameRepo = Sl.R<GameRepo>();
        var game = gameRepo.GetById(gameId);
        game.WillStartAt = DateTime.Now.AddSeconds(4);
        gameRepo.Update(game);
        gameRepo.Flush();

        Sl.R<GameHubConnection>().SendChangeStartTime(gameId);
    }
}