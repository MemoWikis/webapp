using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class UserSettingsController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Account/Settings/UserSettings.aspx";

    private readonly UserRepo _userRepo;

    public UserSettingsController(UserRepo userRepo){
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

        _sessionUser.User.EmailAddress = model.Email.Trim();
        _sessionUser.User.Name = model.Name.Trim();
        _sessionUser.User.AllowsSupportiveLogin = model.AllowsSupportiveLogin;
        _sessionUser.User.ShowWishKnowledge = model.ShowWishKnowledge;
        _sessionUser.User.KnowledgeReportInterval = model.KnowledgeReportInterval;

        _userRepo.Update(_sessionUser.User);
        ReputationUpdate.ForUser(_sessionUser.User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        var newUserSettingsModel = new UserSettingsModel(_sessionUser.User);
        newUserSettingsModel.Message = new SuccessMessage("Deine Änderungen wurden gespeichert");

        return View(_viewLocation, newUserSettingsModel);
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return UserSettings();
    }
}