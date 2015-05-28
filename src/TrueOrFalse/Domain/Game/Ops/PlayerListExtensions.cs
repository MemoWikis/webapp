using System.Collections.Generic;
using System.Linq;

public static class PlayerListExtensions
{
    public static Player Creator(this IList<Player> players)
    {
        return players.First(p => p.IsCreator);
    }
}