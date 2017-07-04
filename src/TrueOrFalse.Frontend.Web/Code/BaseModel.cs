﻿using System;
using System.Web;

public class BaseModel : BaseResolve
{
    public SponsorModel SponsorModel
    {
        get
        {
            if (_sponsorModel != null)
            {
                return _sponsorModel;
            }

            _sponsorModel = new SponsorModel();
            return _sponsorModel;
        }

        set => _sponsorModel = value;
    }

    private SponsorModel _sponsorModel;

    protected SessionUser _sessionUser => Resolve<SessionUser>();
    protected SessionUiData _sessionUiData => Resolve<SessionUiData>();

    public bool IsLoggedIn => _sessionUser.IsLoggedIn;
    public bool IsInstallationAdmin => _sessionUser.IsInstallationAdmin;

    public int UserId => _sessionUser.UserId;

    public Game UpcomingGame;
    
    public bool IsInGame;

    public bool IsCreatorOfGame;

    public MenuLeftModel MenuLeftModel = new MenuLeftModel();

    public bool IsThemeNavigationPage;

    public bool ShowUserReportWidget = true;

    public BaseModel()
    {
        if (IsLoggedIn)
        {
            var isInGame = R<IsCurrentlyIn>().Game(UserId);
            if (isInGame.Yes)
            {
                UpcomingGame = isInGame.Game;
                IsInGame = true;
                IsCreatorOfGame = isInGame.IsCreator;
            }
            else
                UpcomingGame = new Game();
        }
        SetLeftMenuData();
    }

    private void SetLeftMenuData()
    {
        var httpContextData = HttpContext.Current.Request.RequestContext.RouteData.Values;
        var isThemePageRequest = (string)httpContextData["controller"] == "Category" &&
                                 (string)httpContextData["action"] == "Category";
        var isMainPageRequest = (string)httpContextData["controller"] == "Welcome" &&
                                (string)httpContextData["action"] == "Welcome";
        if (isThemePageRequest || isMainPageRequest)
            IsThemeNavigationPage = true;

        if (isThemePageRequest)
            MenuLeftModel.ActualCategory = Sl.CategoryRepo.GetById(Convert.ToInt32(httpContextData["id"]));
    }
}