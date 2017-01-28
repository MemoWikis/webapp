using System.Net;

public class IsFacebookAccessToken
{
    public static bool Valid(string accessToken, string facebookUserId)
    {
        var url = $"https://graph.facebook.com/app?access_token={accessToken}";

        return new WebClient()
            .DownloadString(url)
            .Contains(facebookUserId);
    }
}