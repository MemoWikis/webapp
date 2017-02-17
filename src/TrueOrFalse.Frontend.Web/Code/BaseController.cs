﻿using System.Web.Mvc;

public class BaseController : Controller
{
    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public int UserId => _sessionUser.UserId;

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;
    public bool IsMemuchoUser => IsLoggedIn && Settings.MemuchoUserId == UserId;
    public bool IsFacebookUser => IsLoggedIn && _sessionUser.User.IsFacebookUser();

    /// <summary>The user fresh from the db</summary>
    public User User_() => R<UserRepo>().GetById(UserId);
    public User MemuchoUser() => R<UserRepo>().GetById(Settings.MemuchoUserId);

    protected T Resolve<T>() => ServiceLocator.Resolve<T>();

    protected T R<T>() => ServiceLocator.Resolve<T>();
}