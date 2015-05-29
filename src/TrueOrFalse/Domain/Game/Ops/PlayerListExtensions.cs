using System.Collections.Generic;
using System.Linq;

public static class PlayerListExtensions
{
    public static Player Creator(this IList<Player> players)
    {
        return players.First(p => p.IsCreator);
    }

    public static Player ByUserId(this IList<Player> players, int userId)
    {
        return players.FirstOrDefault(p => p.User.Id == userId);
    }

    public static bool IsPlayer(this IList<Player> players, int userId)
    {
        return players.Any(p => p.User.Id == userId);
    }
}