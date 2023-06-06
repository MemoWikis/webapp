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

    public UserSettingsController(UserRepo userRepo, SessionUser sessionUser) : base(sessionUser){
        _userRepo = userRepo;
    }

    [HttpGet]
    [SetMainMenu(MainMenuEntry.None)]
    public ViewResult UserSettings()
    {
        var userSettingsModel = new UserSettingsModel(SessionUserLegacy.User);
        if ((Request["update"] != null) && (Request["token"] != null))
        {
            if (Request["update"] == UpdateKnowledgeReportInterval.CommandName)
            {
                var result = UpdateKnowledgeReportInterval.Run(Request["userId"].ToInt(), Request["val"].ToInt(), Request["expires"], Request["token"]);
                userSettingsModel.Message = result.ResultMessage;
                if (result.Success && ((UserCacheItem)SessionUserLegacy.User).Id == result.AffectedUser.Id)
                {
                    SessionUserLegacy.User.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
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

        SessionUserLegacy.User.EmailAddress = model.Email.Trim();
        SessionUserLegacy.User.Name = model.Name.Trim();
        SessionUserLegacy.User.AllowsSupportiveLogin = model.AllowsSupportiveLogin;
        SessionUserLegacy.User.ShowWishKnowledge = model.ShowWishKnowledge;
        SessionUserLegacy.User.KnowledgeReportInterval = model.KnowledgeReportInterval;

        EntityCache.AddOrUpdate(SessionUserLegacy.User);

        _userRepo.Update(SessionUserLegacy.User);
        ReputationUpdate.ForUser(SessionUserLegacy.User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        var newUserSettingsModel = new UserSettingsModel(SessionUserLegacy.User);
        newUserSettingsModel.Message = new SuccessMessage("Deine Änderungen wurden gespeichert");

        return View(_viewLocation, newUserSettingsModel);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, SessionUserLegacy.UserId);
        return UserSettings();
    }
}