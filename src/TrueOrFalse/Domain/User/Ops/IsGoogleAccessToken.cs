using System.Net;

public class IsGoogleAccessToken
{
    public static bool Valid(string googleToken)
    {
        var url = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={googleToken}";

        var result = (HttpWebRequest)WebRequest.Create(url);
        var response = (HttpWebResponse)result.GetResponse();

        return response.StatusCode == HttpStatusCode.OK;
    }
}