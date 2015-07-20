public class GameCreate : IRegisterAsInstancePerLifetime
{
    public void Run(Game game, bool multipleChoiceOnly = false)
    {
        var gameRepo = Sl.Resolve<GameRepo>();
        gameRepo.Create(game);
        gameRepo.Flush();

        Sl.R<AddRoundsToGame>().Run(game, multipleChoiceOnly);

        gameRepo.Update(game);
    }
}