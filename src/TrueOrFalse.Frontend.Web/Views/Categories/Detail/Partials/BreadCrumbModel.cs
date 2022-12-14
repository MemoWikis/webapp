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
        User = SessionUser.User;
        if (IsLoggedIn)
        {
            UserName = SessionUser.User.Name;
            var imageSetttings = new UserImageSettings(SessionUser.UserId);
            UserImage = imageSetttings.GetUrl_30px_square(SessionUser.User).Url;
            ToolTipToHomepage = "Zu deinem Wiki";
        }
    }
}