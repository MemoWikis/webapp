using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;
using TrueOrFalse.Frontend.Web.Models;

public class UserProfileModel : ModelBase
{
    public UserProfileModel(User user)
    {
        Name = user.Name;
    }

    public string Name { get; private set; }
    public bool IsCurrentUserProfile;
}
