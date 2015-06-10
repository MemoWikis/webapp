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

    [SetMenu(MenuEntry.UserDetail)]
    new public ViewResult User(string userName, int id)
    {
        var user = _userRepo.GetById(id);
        _sessionUiData.VisitedUserDetails.Add(new UserHistoryItem(user));

        return View(_viewLocation, new UserModel(user));
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return User(_sessionUser.User.Name, _sessionUser.User.Id);
    }
}