
using System.Collections.Generic;
using System.Linq;

public class BadgeTypeGroup
{
    public string Key;
    public string Title;

    public BadgeTypeGroup(string key, string title)
    {
        Key = key;
        Title = title;
    }

    private static IList<BadgeTypeGroup> _allGroups;
    public static IList<BadgeTypeGroup> GetAll()
    {
        return _allGroups ?? (_allGroups = new List<BadgeTypeGroup>
        {
            new BadgeTypeGroup(BadgeTypeGroupKeys.FirstSteps, "Erste Schritte"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Questions, "Fragen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Sets, "Fragesätzen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.WishKnowledge, "Wunschwissen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Training, "Spielen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Play, "Spielen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Community, "Community"),
        });
    }

    public static BadgeTypeGroup GetByKey(string key)
    {
        return GetAll().First(x => x.Key == key);
    }
}

public class BadgeTypeGroupKeys
{
    public const string FirstSteps = "FirstSteps";
    public const string Questions = "Questions";
    public const string Sets = "Sets";
    public const string Categories = "Categories";
    public const string WishKnowledge = "WishKnowledge";
    public const string Training = "Training";
    public const string Play = "Play";
    public const string Community = "Community";
}