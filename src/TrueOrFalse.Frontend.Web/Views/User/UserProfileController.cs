using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class UserProfileController : Controller
{
    private const string _viewLocation = "~/Views/User/UserProfile.aspx";

    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public UserProfileController(UserRepository userRepository, SessionUiData sessionUiData, SessionUser sessionUser)
    {
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    [SetMenu(MenuEntry.ProfilDetail)]
    public ViewResult Profile(string userName, int id)
    {
        var user = _userRepository.GetById(id);
        _sessionUiData.VisitedProfiles.Add(new UserHistoryItem(user));

        var imageResult = new ProfileImageSettings(user.Id).GetUrl_128px(user.EmailAddress);
        return View(_viewLocation, new UserProfileModel(user)
                                       {
                                           IsCurrentUserProfile = _sessionUser.User == user,
                                           ImageUrl_128 = imageResult.Url,
                                           ImageIsCustom = imageResult.HasUploadedImage
                                       });
    }

    [HttpPost]
    public ViewResult UploadProfilePicture(HttpPostedFileBase file)
    {
        ProfileImageStore.Run(file, _sessionUser.User.Id);
        return Profile(_sessionUser.User.Name, _sessionUser.User.Id);
    }
}