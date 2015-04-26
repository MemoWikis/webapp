using System;

public class SignalRAuth
{
    public static string Login(User user)
    {
        var signalrAuth = new SignalRAuthInfo();
        signalrAuth.UserId = user.Id;
        signalrAuth.CookieToken = Guid.NewGuid().ToString();
        SignalRAuthTokenRepo.Insert(signalrAuth);

        return signalrAuth.CookieToken;
    }

    public static void Logout(string cookieToken)
    {
        SignalRAuthTokenRepo.Remove(cookieToken);
    }
}

