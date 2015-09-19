using System.Collections.Generic;

public class BadgeType
{
    public string Key;

    public string Name;
    public string Description;

    public bool IsSecret;

    public IList<BadgeLevel> Levels = new List<BadgeLevel>();

    public BadgeTypeGroup Group;

    public static IList<BadgeType> GetAll()
    {
        return new[]
        {
            new BadgeType
            {
                Key = "FirstHourSupporter",
                Name = "Fördermitglied der 1. Stunde",
                Description = "Während des 1. Jahres Fördermitglied geworden (und für ein Jahr geblieben)",
                Group =  BadgeTypeGroup.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()}
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Nutzer der 1. Stunde",
                Description = "Während der Beta-Phase Nutzer geworden",
                Group =  BadgeTypeGroup.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()}
            },
        };
    }
}