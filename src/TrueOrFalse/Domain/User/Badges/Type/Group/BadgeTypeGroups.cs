using System.Collections.Generic;
using System.Linq;

public class BadgeTypeGroups
{
    private static IList<BadgeTypeGroup> _allGroups;
    public static IList<BadgeTypeGroup> All()
    {
        return _allGroups ?? (_allGroups = new List<BadgeTypeGroup>
        {
            new BadgeTypeGroup(BadgeTypeGroupKeys.FirstSteps, "Erste Schritte"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Questions, "Fragen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Sets, "Lernsets"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Categories, "Themen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.WishKnowledge, "Wunschwissen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Training, "Lernen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Play, "Spielen"),
            new BadgeTypeGroup(BadgeTypeGroupKeys.Community, "Community"),
        });
    }

    public static BadgeTypeGroup GetByKey(string key)
    {
        return All().First(x => x.Key == key);
    }
}