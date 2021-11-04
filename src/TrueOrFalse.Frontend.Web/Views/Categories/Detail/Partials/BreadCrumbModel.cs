using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrueOrFalse.Web;

public class BreadCrumbModel : BaseModel
{
    public User User { get; set; }
    public string UserImage = "";
    public string UserName = "";
    public string ToolTipToHomepage = "Zur Startseite";
    public BreadCrumbModel()
    {
        User = _sessionUser.User;
        if (IsLoggedIn)
        {
            UserName = _sessionUser.User.Name;
            var imageSetttings = new UserImageSettings(_sessionUser.User.Id);
            UserImage = imageSetttings.GetUrl_30px_square(_sessionUser.User).Url;
            ToolTipToHomepage = "Zu deinem Wiki";
        }
    }
}