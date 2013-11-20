using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;

public class UsersController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Users.aspx";

    private readonly UsersControllerSearch _usersControllerSearch;

    public UsersController(UsersControllerSearch usersControllerSearch){
        _usersControllerSearch = usersControllerSearch;
    }

    public ActionResult Search(string searchTerm, UsersModel model)
    {
        _sessionUiData.SearchSpecUser.SearchTerm = model.SearchTerm = searchTerm;
        return Users(null, model);
    }

    [SetMenu(MenuEntry.Users)]
    public ActionResult Users(int? page, UsersModel model)
    {
        _sessionUiData.SearchSpecUser.PageSize = 10;
        if (page.HasValue) _sessionUiData.SearchSpecUser.CurrentPage = page.Value;

        
        if(model == null)
            model = new UsersModel();

        var users = _usersControllerSearch.Run();
        model.Init(users);


        return View(_viewLocation, model);
    }

    [AccessOnlyAsAdmin]
    public ActionResult LoginAs(int userId)
    {
        var user = Resolve<UserRepository>().GetById(userId);
        _sessionUser.Login(user);
        _sessionUser.IsInstallationAdmin = true;

        var model = new UsersModel();
        model.Message = new SuccessMessage("Nun Bist Du angemeldet als <b>\"" + user.Name +  "\"</b>");
        return Users(null, model);
    }
}