using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse.Web;

public class BreadCrumbModel : BaseModel
{
    public UserCacheItem User { get; set; }
    public string UserImage = "";
    public string UserName = "";
    public string ToolTipToHomepage = "Zur Startseite";
    public BreadCrumbModel(TopNavMenu topNavMenu)
    {
        TopNavMenu = topNavMenu;
        User = SessionUserLegacy.User;
        if (IsLoggedIn)
        {
            UserName = SessionUserLegacy.User.Name;
            var imageSetttings = new UserImageSettings(SessionUserLegacy.UserId);
            UserImage = imageSetttings.GetUrl_30px_square(SessionUserLegacy.User).Url;
            ToolTipToHomepage = "Zu deinem Wiki";
        }
    }
}