using System.Collections.Generic;

public class TabBadgesModel : BaseModel
{
    public UserTinyModel User;

    public IList<BadgeTypeGroup> BadgeTypeGroups;
    public IList<BadgeType> BadgeTypes;

    public TabBadgesModel(UserModel userModel)
    {
        User = userModel.User;

        BadgeTypes = global::BadgeTypes.All();
        BadgeTypeGroups = global::BadgeTypeGroups.All();
    }
}