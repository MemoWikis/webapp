using System.Collections.Generic;

public class TabBadgesModel : BaseModel
{
    public User User;

    public IList<BadgeTypeGroup> BadgeTypeGroups;
    public IList<BadgeType> BadgeTypes;

    public TabBadgesModel(UserModel userModel)
    {
        User = userModel.User;

        BadgeTypes = global::BadgeTypes.GetAll();
        BadgeTypeGroups = global::BadgeTypeGroups.GetAll();
    }
}