public class HtmlMessageModel
{
    public string UserName;
    public string Content;

    public string MessageTitle;

    public string UtmCampaign;
    public string UtmCampaignFullString
    {
        get
        {
            if (string.IsNullOrEmpty(UtmCampaign))
                return "";

            return "&utm_campaign=" + UtmCampaign;
        }
    }

    public string UtmSource;
    public string UtmSourceFullString
    {
        get
        {
            if (string.IsNullOrEmpty(UtmSource))
                return "&utm_source=automaticEmail";

            return "&utm_source=" + UtmSource;
        }
    }

    public string SignOutMessage;

}