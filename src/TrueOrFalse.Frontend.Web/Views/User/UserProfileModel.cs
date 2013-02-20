using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class UserProfileModel : BaseModel
{
    public UserProfileModel(User user)
    {
        Name = user.Name;
    }

    public string Name { get; private set; }

    public string ImageUrl_128;
    public bool ImageIsCustom;

    public bool IsCurrentUserProfile;
}
