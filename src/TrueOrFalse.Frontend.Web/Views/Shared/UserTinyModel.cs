using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class UserTinyModel
{
    public string Name;
    public string ImageUrl; 


    private readonly User _user;

    public UserTinyModel(User user)
    {
        _user = user;


        if (_user == null)
        {
            Name = "Unbekannt";
        }
        else
        {
            Name = _user.Name;

        }

    }

    
}
