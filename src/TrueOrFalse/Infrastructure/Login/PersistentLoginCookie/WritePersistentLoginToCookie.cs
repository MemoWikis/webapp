using System;
using System.Web;

public class WritePersistentLoginToCookie : IRegisterAsInstancePerLifetime
{
    private readonly CreatePersistentLogin _createPersistentLogin;

    public WritePersistentLoginToCookie(CreatePersistentLogin createPersistentLogin)
    {
        _createPersistentLogin = createPersistentLogin;
    }

    public void Run(int userId)
    {
        var loginGuid = _createPersistentLogin.Run(userId);

        var cookie = MemuchoCookie.GetNew();
        cookie.Values.Add("persistentLogin", userId + "-x-" + loginGuid);
        HttpContext.Current.Response.Cookies.Add(cookie);
    }        
}