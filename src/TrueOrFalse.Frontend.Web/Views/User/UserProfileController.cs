using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

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

    public ViewResult Profile(string userName, int id)
    {
        var user = _userRepository.GetById(id);
        _sessionUiData.LastVisitedProfiles.Add(new UserNavigationModel(user));
        return View(_viewLocation, new UserProfileModel(user) {IsCurrentUserProfile = _sessionUser.User == user});
    }

    public ViewResult UploadProfilePicture()
    {
        return Profile(_sessionUser.User.Name, _sessionUser.User.Id);
    }

}
