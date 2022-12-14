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
        var userSettingsModel = new UserSettingsModel(SessionUser.User);
        if ((Request["update"] != null) && (Request["token"] != null))
        {
            if (Request["update"] == UpdateKnowledgeReportInterval.CommandName)
            {
                var result = UpdateKnowledgeReportInterval.Run(Request["userId"].ToInt(), Request["val"].ToInt(), Request["expires"], Request["token"]);
                userSettingsModel.Message = result.ResultMessage;
                if (result.Success && ((UserCacheItem)SessionUser.User).Id == result.AffectedUser.Id)
                {
                    SessionUser.User.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
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

        SessionUser.User.EmailAddress = model.Email.Trim();
        SessionUser.User.Name = model.Name.Trim();
        SessionUser.User.AllowsSupportiveLogin = model.AllowsSupportiveLogin;
        SessionUser.User.ShowWishKnowledge = model.ShowWishKnowledge;
        SessionUser.User.KnowledgeReportInterval = model.KnowledgeReportInterval;

        EntityCache.AddOrUpdate(SessionUser.User);

        _userRepo.Update(SessionUser.User);
        ReputationUpdate.ForUser(SessionUser.User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        var newUserSettingsModel = new UserSettingsModel(SessionUser.User);
        newUserSettingsModel.Message = new SuccessMessage("Deine Änderungen wurden gespeichert");

        return View(_viewLocation, newUserSettingsModel);
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, SessionUser.UserId);
        return UserSettings();
    }
}