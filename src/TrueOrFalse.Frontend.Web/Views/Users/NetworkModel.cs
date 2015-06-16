using System.Collections.Generic;
using System.Linq;

public class NetworkModel : BaseModel
{
    public int TotalUsers;

    public IEnumerable<UserRowModel> UserIFollow = new List<UserRowModel>();
    public IEnumerable<UserRowModel> UsersFollowingMe = new List<UserRowModel>();

    public HeaderModel HeaderModel  = new HeaderModel();

    public NetworkModel()
    {
        HeaderModel.TotalUsers = R<GetTotalUsers>().Run();
        HeaderModel.IsNetworkTab = true;

        if (!IsLoggedIn)
            return;

        var user = R<UserRepo>().GetById(UserId);

        var followerIAm = R<FollowerIAm>().Init(user.Following.Select(u => u.Id), UserId);

        UserIFollow = user.Following.Select(u => new UserRowModel(u, -1, followerIAm));
        UsersFollowingMe = user.Followers.Select(u => new UserRowModel(u, -1, followerIAm));

        HeaderModel.TotalIFollow = UserIFollow.Count();
        HeaderModel.TotalFollowingMe = UsersFollowingMe.Count();
    }
}