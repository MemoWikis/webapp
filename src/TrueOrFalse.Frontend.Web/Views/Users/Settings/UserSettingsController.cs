using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class UserSettingsController : Controller
{
    private const string _viewLocation = "~/Views/Users/Settings/UserSettings.aspx";

    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public UserSettingsController(UserRepository userRepository, SessionUiData sessionUiData, SessionUser sessionUser)
    {
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    public ViewResult UserSettings()
    {
        var imageResult = new UserImageSettings(_sessionUser.User.Id).GetUrl_200px(_sessionUser.User.EmailAddress);
        return View(_viewLocation, new UserSettingsModel(_sessionUser.User){
                                           ImageUrl_200 = imageResult.Url,
                                           ImageIsCustom = imageResult.HasUploadedImage
                                       });
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return UserSettings();
    }
}