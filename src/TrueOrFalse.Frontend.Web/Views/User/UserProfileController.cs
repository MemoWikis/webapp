using System;
using System.Collections.Generic;
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

        var getUserImageUrlResult = new GetUserImageUrl().Run(user);
        return View(_viewLocation, new UserProfileModel(user)
                                       {
                                           IsCurrentUserProfile = _sessionUser.User == user,
                                           ImageUrl = getUserImageUrlResult.Url, 
                                           ImageIsCustom = getUserImageUrlResult.HasUploadedImage
                                       });
    }

    [HttpPost]
    public ViewResult UploadProfilePicture(HttpPostedFileBase file)
    {
        new StoreImages().Run(file.InputStream, Server.MapPath("/Images/Users/" + _sessionUser.User.Id));
        return Profile(_sessionUser.User.Name, _sessionUser.User.Id);
    }
}