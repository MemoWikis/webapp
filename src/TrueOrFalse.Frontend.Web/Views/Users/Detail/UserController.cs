﻿using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

public class UserController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Detail/User.aspx";

    private readonly UserRepo _userRepo;

    public UserController(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    [SetMainMenu(MainMenuEntry.UserDetail)]
    [SetUserMenu(UserMenuEntry.UserDetail)]
    public ViewResult User(string userName, int id)
    {
        var user = _userRepo.GetById(id);
        _sessionUiData.VisitedUserDetails.Add(new UserHistoryItem(user));

        return View(_viewLocation, new UserModel(user, isActiveTabKnowledge: true));
    }

    [SetMainMenu(MainMenuEntry.UserDetail)]
    [SetUserMenu(UserMenuEntry.UserDetail)]
    public ViewResult Badges(string userName, int id)
    {
        var user = _userRepo.GetById(id);
        _sessionUiData.VisitedUserDetails.Add(new UserHistoryItem(user));

        return View(_viewLocation, new UserModel(user, isActiveTabBadges: true));
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return User(_sessionUser.User.Name, _sessionUser.User.Id);
    }

    [HttpPost]
    public bool SetUserWorldInUserCache(bool showMyWorld)
    {
        if (!IsLoggedIn)
            return false;

        UserCache.GetItem(_sessionUser.UserId).IsFiltered = showMyWorld;

        return showMyWorld; 
    }

    [HttpPost]
    public bool IsFiltered()
    {
        return UserCache.GetItem(_sessionUser.UserId).IsFiltered;
    }
}