public class CancelGame : IRegisterAsInstancePerLifetime
{
    public void Run(int gameId)
    {
        var gameRepo = Sl.R<GameRepo>();
        var game = gameRepo.GetById(gameId);
        game.Status = GameStatus.Canceled;
        gameRepo.Update(game);
        gameRepo.Flush();

        Sl.R<GameHubConnection>().SendCancelation(gameId);
    }
}