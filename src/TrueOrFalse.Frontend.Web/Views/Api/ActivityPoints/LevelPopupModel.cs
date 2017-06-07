public class LevelPopupModel
{
    public int UserLevel;
    public bool IsLoggedIn;

    public LevelPopupModel(int level, bool isLoggedIn)
    {
        UserLevel = level;
        IsLoggedIn = isLoggedIn;
    }
}