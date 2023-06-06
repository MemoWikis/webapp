public class CreatePersistentLogin
{
    public static string Run(int userId)
    {
        var newGuid = Guid.NewGuid().ToString();
        var persistentLogin = new PersistentLogin { UserId = userId, LoginGuid = newGuid };
        Sl.R<PersistentLoginRepo>().Create(persistentLogin);
        return newGuid;
    }
}