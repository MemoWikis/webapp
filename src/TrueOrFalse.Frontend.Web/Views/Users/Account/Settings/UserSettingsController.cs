using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class UserSettingsController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Account/Settings/UserSettings.aspx";

    private readonly UserRepository _userRepo;

    public UserSettingsController(UserRepository userRepo){
        _userRepo = userRepo;
    }

    [HttpGet]
    public ViewResult UserSettings()
    {
        return View(_viewLocation, new UserSettingsModel(_sessionUser.User));
    }

    [HttpPost]
    public ViewResult UserSettings(UserSettingsModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Message = new ErrorMessage(ModelState);
            return View(_viewLocation, model);
        }

        model.Message = new SuccessMessage("Wurde gespeichert");

        _sessionUser.User.EmailAddress = model.Email.Trim();
        _sessionUser.User.Name = model.Name.Trim();
        _sessionUser.User.AllowsSupportiveLogin = model.AllowsSupportiveLogin;
        _sessionUser.User.ShowWishKnowledge = model.ShowWishKnowledge;

        _userRepo.Update(_sessionUser.User);

        return View(_viewLocation, new UserSettingsModel(_sessionUser.User));
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return UserSettings();
    }
}