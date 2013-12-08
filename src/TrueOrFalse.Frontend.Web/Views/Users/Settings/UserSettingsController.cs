using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TrueOrFalse;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

public class UserSettingsController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Settings/UserSettings.aspx";

    private readonly UserRepository _userRepo;

    public UserSettingsController(UserRepository userRepo){
        _userRepo = userRepo;
    }

    [HttpGet]
    public ViewResult UserSettings()
    {
        var imageResult = new UserImageSettings(_sessionUser.User.Id).GetUrl_200px(_sessionUser.User.EmailAddress);
        return View(_viewLocation, new UserSettingsModel(_sessionUser.User){
                                           ImageUrl_200 = imageResult.Url,
                                           ImageIsCustom = imageResult.HasUploadedImage
                                       });
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

        _userRepo.Update(_sessionUser.User);

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return UserSettings();
    }
}