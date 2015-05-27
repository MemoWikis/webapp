using System.Web;
using System.Web.Mvc;

public class UserController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Detail/User.aspx";

    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [SetMenu(MenuEntry.UserDetail)]
    new public ViewResult User(string userName, int id)
    {
        var user = _userRepository.GetById(id);
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