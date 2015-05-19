public class PlayModel : BaseModel
{
    public Game Game;

    public int RoundCount;

    public PlayModel(Game game)
    {
        Game = game;
        RoundCount = game.RoundCount;
    }
}