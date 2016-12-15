using System.Collections.Generic;
using System.Web.Mvc;

public class WelcomeModel : BaseModel
{

    public string Date;

    public bool UserHasWishknowledge => WishCount > 0;
    public int WishCount = 0;
    
    public WelcomeModel()
    {
        if (IsLoggedIn)
        {
            WishCount = _sessionUser.User.WishCountQuestions;
        }
    }


}
