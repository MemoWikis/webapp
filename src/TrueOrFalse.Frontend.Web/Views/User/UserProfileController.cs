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

    public UserProfileController(UserRepository userRepository, SessionUiData sessionUiData)
    {
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
    }

    public ViewResult Profile(string userName, int id)
    {
        var user = _userRepository.GetById(id);

        _sessionUiData.LastVisitedProfiles.Add(new UserNavigationModel(user));

        return View(_viewLocation, new UserProfileModel(user));
    }

}
