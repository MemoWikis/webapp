﻿using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;
using TrueOrFalse.Frontend.Web.Models;

public class RegisterModelToUser : ModelBase
{
    public static User Run(RegisterModel registerModel)
    {
        var user = new User();
        user.EmailAddress = registerModel.Email;
        user.Name = registerModel.Name;

        SetUserPassword.Run(registerModel.Password, user);

        return user;
    }
}
