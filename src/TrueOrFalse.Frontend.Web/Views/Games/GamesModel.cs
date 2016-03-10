using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Infrastructure;

public class GamesModel : BaseModel
{
    public List<GameRowModel> GamesReady;
    public List<GameRowModel> GamesInProgress;

    public List<Set> SuggestedGames;

    public GamesModel(IList<Game> games)
    {
        SuggestedGames = Sl.R<DbSettingsRepo>().Get().SuggestedGameSets();

        GamesReady = games.Where(g => g.Status == GameStatus.Ready).Select(g => new GameRowModel(g)).ToList();
        GamesInProgress = games.Where(g => g.Status == GameStatus.InProgress).Select(g => new GameRowModel(g)).ToList();
    }
}