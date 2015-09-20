using System.Collections.Generic;

public class BadgeType
{
    public string Key;

    public string Name;
    public string Description;

    public bool IsSecret;

    public IList<BadgeLevel> Levels = new List<BadgeLevel>();

    public BadgeTypeGroup Group;
}