using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class UserModel : BaseModel
{
    public UserModel(User user)
    {
        Name = user.Name;
    }

    public string Name { get; private set; }

    public string ImageUrl_200;
    public bool ImageIsCustom;

    public bool IsCurrentUser;
}
