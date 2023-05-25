using System.Web;
using System.Web.Mvc;

public class UserController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Detail/User.aspx";

    private readonly UserRepo _userRepo;

    public UserController(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, SessionUser.UserId);
        return User(SessionUser.User.Name, SessionUser.UserId);
    }

    [SetMainMenu(MainMenuEntry.UserDetail)]
    [SetUserMenu(UserMenuEntry.UserDetail)]
    public ViewResult User(string userName, int id)
    {
        var user = EntityCache.GetUserById(id);
        _sessionUiData.VisitedUserDetails.Add(new UserHistoryItem(user));

        return View(_viewLocation, new UserModel(user, true));
    }
}