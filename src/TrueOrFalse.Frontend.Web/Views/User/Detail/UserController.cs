﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class UserController : Controller
{
    private const string _viewLocation = "~/Views/User/Detail/User.aspx";

    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public UserController(UserRepository userRepository, SessionUiData sessionUiData, SessionUser sessionUser)
    {
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    [SetMenu(MenuEntry.UserDetail)]
    public ViewResult User(string userName, int id)
    {
        var user = _userRepository.GetById(id);
        _sessionUiData.VisitedUserDetails.Add(new UserHistoryItem(user));

        var imageResult = new UserImageSettings(user.Id).GetUrl_128px(user.EmailAddress);
        return View(_viewLocation, new UserModel(user)
                                       {
                                           IsCurrentUser = _sessionUser.User == user,
                                           ImageUrl_128 = imageResult.Url,
                                           ImageIsCustom = imageResult.HasUploadedImage
                                       });
    }

    [HttpPost]
    public ViewResult UploadPicture(HttpPostedFileBase file)
    {
        UserImageStore.Run(file, _sessionUser.User.Id);
        return User(_sessionUser.User.Name, _sessionUser.User.Id);
    }
}