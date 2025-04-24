using Newtonsoft.Json.Linq;

public class IsFacebookAccessToken
{
    private static readonly HttpClient _client = new HttpClient();

    public static async Task<bool> IsAccessTokenValidAsync(
        string accessToken,
        string facebookUserId)
    {
        try
        {
            var response = await _client.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={Settings.FacebookAppId}|{Settings.FacebookAppSecret}");
            var jsonObject = JObject.Parse(response);

            if (jsonObject["data"]?["app_id"]?.Value<string>() == Settings.FacebookAppId &&
                jsonObject["data"]?["user_id"]?.Value<string>() == facebookUserId &&
                jsonObject["data"]?["is_valid"]?.Value<bool>() == true)
                return true;
        }
        catch (Exception ex)
        {
            Logg.r.Error("FB Access Token Verification - {msg}", ex.Message);
            return false;
        }

        return false;
    }
}