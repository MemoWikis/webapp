public class LoginFromCookie : IRegisterAsInstancePerLifetime
{
    private readonly GetPersistentLoginCookieValues _getPersistentLoginCookieValues;
    private readonly WritePersistentLoginToCookie _writePersistentLoginToCookie;
    private readonly PersistentLoginRepository _persistentLoginRepository;
    private readonly SessionUser _sessionUser;
    private readonly UserRepo _userRepo;

    public LoginFromCookie(GetPersistentLoginCookieValues getPersistentLoginCookieValues,
                            WritePersistentLoginToCookie writePersistentLoginToCookie,
                            PersistentLoginRepository persistentLoginRepository, 
                            SessionUser sessionUser, 
                            UserRepo userRepo)
    {
        _getPersistentLoginCookieValues = getPersistentLoginCookieValues;
        _writePersistentLoginToCookie = writePersistentLoginToCookie;
        _persistentLoginRepository = persistentLoginRepository;
        _sessionUser = sessionUser;
        _userRepo = userRepo;
    }

    public bool Run()
    {
        var cookieValues = _getPersistentLoginCookieValues.Run();

        if (!cookieValues.Exists())
            return false;

        var persistentLogin = _persistentLoginRepository.Get(cookieValues.UserId, cookieValues.LoginGuid);

        if (persistentLogin == null)
            return false;

        var user = _userRepo.GetById(cookieValues.UserId);
        if (user == null)
            return false;

        _persistentLoginRepository.Delete(persistentLogin);
        _writePersistentLoginToCookie.Run(cookieValues.UserId);

        _sessionUser.Login(user);            

        return true;
    }
}