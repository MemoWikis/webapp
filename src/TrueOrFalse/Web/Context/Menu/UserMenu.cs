using System;

public enum UserMenuEntry
{
    None, Messages, 
    Network, UserProfile, 
    UserSettings
}

[Serializable]
public class UserMenu
{
    public UserMenuEntry Current = UserMenuEntry.None;

    public string Active(UserMenuEntry userMenuEntry)
    {
        if (Current == userMenuEntry)
            return "active";

        return "";
    }
}