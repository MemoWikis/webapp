using System.Collections.Generic;
using System.Linq;

public class NetworkModel : BaseModel
{
    public int TotalUsers;

    public int TotalIFollow { get { return UserIFollow.Count(); } }
    public int TotalFollowingMe{ get { return UsersFollowingMe.Count(); } }

    public IEnumerable<UserRowModel> UserIFollow = new List<UserRowModel>();
    public IEnumerable<UserRowModel> UsersFollowingMe = new List<UserRowModel>();

    public NetworkModel()
    {
        TotalUsers = R<GetTotalUsers>().Run();

        if (!IsLoggedIn)
            return;

        var user = R<UserRepo>().GetById(UserId);

        var followerIAm = R<FollowerIAm>().Init(user.Following.Select(u => u.Id), UserId);

        UserIFollow = user.Following.Select(u => new UserRowModel(u, -1, followerIAm));
        UsersFollowingMe = user.Followers.Select(u => new UserRowModel(u, -1, followerIAm));
    }
}