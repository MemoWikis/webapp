using System.Net;

public class IsFacebookAccessToken
{
    public static bool Valid(string accessToken, string facebookUserId)
    {
        var appUrl = $"https://graph.facebook.com/app?access_token={accessToken}";
        var appIdIsValid = new WebClient()
            .DownloadString(appUrl)
            .Contains(Settings.FacebookAppId);


        var userUrl = $"https://graph.facebook.com/me?access_token={accessToken}";
        var userIdIsValid = new WebClient()
            .DownloadString(userUrl)
            .Contains(facebookUserId);

        return appIdIsValid && userIdIsValid;
    }
}