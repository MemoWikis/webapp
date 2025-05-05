public class CreatePersistentLogin
{
    public static string Run(int userId, PersistentLoginRepo persistentLoginRepo)
    {
        var newGuid = Guid.NewGuid().ToString();
        var persistentLogin = new PersistentLogin { UserId = userId, LoginGuid = newGuid };
        persistentLoginRepo.Create(persistentLogin);
        return newGuid;
    }
}