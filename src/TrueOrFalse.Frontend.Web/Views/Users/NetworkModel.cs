using System.Collections.Generic;
using System.Linq;

public class NetworkModel : BaseModel
{

    public int TotalUsers;

    public int TotalIFollowers { get { return UserIFollow.Count(); } }
    public int TotalFollowingMe{ get { return UsersFollowingMe.Count(); } }

    public IEnumerable<UserRowModel> UserIFollow = new List<UserRowModel>();
    public IEnumerable<UserRowModel> UsersFollowingMe = new List<UserRowModel>();
}