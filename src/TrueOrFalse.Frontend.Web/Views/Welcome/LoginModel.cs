﻿using TrueOrFalse.Core.Web;
using TrueOrFalse.Frontend.Web.Models;

public class LoginModel : ModelBase
{
    public Message Message;

    public string UserName;
    public string Password;

    public LoginModel()
    {
        ShowLeftMenu_Empty();
    }

    public void SetToWrongCredentials()
    {
        Message = new Message(MessageType.IsError, 
            "Du konntest nicht angemeldet werden. Bitte überprüfe Deinen Benutzernamen und Passwort"); ;
    }
}
