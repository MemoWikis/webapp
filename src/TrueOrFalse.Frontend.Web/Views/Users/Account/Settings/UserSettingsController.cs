using System;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Web;

[SetUserMenu(UserMenuEntry.UserSettings)]
public class UserSettingsController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Account/Settings/UserSettings.aspx";

    private readonly UserRepo _userRepo;

    public UserSettingsController(UserRepo userRepo){
        _userRepo = userRepo;
    }

    [HttpGet]
    [SetMainMenu(MainMenuEntry.None)]
    public ViewResult UserSettings()
    {
        var userSettingsModel = new UserSettingsModel(_sessionUser.User);
        if ((Request["update"] != null) && (Request["token"] != null))
        {
            if (Request["update"] == UpdateKnowledgeReportInterval.CommandName)
            {
                var result = UpdateKnowledgeReportInterval.Run(Request["userId"].ToInt(), Request["val"].ToInt(), Request["expires"], Request["token"]);
                userSettingsModel.Message = result.ResultMessage;
                if (result.Success && (_sessionUser.User == result.AffectedUser))
                {
                    _sessionUser.User.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
                    userSettingsModel.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
                }
            }
        }

        return View(_viewLocation, userSettingsModel);
    }

    [HttpPost]
    [SetMainMenu(MainMenuEntry.None)]
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