
using System.Linq;
using System.Web.Mvc;

namespace VueApp;
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class GridLogic : IRegisterAsInstancePerLifetime
{
    private readonly PermissionCheck _permissionCheck;
    private readonly int _sessionUserId;

    public GridLogic(PermissionCheck permissionCheck, SessionUser sessionUser)
    {
        _permissionCheck = permissionCheck;
        _sessionUserId = sessionUser.UserId;
    }
}






