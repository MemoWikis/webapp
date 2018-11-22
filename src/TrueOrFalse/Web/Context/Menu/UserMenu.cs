using System;

public enum UserMenuEntry
{
    None, Messages, 
    Network, UserDetail, 
    UserSettings
}

[Serializable]
public class UserMenu
{
    public UserMenuEntry Current = UserMenuEntry.None;

    public string UserMenuActive(UserMenuEntry userMenuEntry)
    {
        if (Current == userMenuEntry)
            return "active";

        return "";
    }
}