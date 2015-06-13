using System.Collections.Generic;
using System.Linq;

public class GamesModel : BaseModel
{
    public List<GameRowModel> GamesReady;
    public List<GameRowModel> GamesInProgress;

    public GamesModel(IList<Game> games)
    {
        GamesReady = games.Where(g => g.Status == GameStatus.Ready).Select(g => new GameRowModel(g)).ToList();
        GamesInProgress = games.Where(g => g.Status == GameStatus.InProgress).Select(g => new GameRowModel(g)).ToList();
    }
}