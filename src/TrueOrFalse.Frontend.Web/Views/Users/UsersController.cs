using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class UsersController : BaseController
{
    private const string _viewLocation = "~/Views/Users/Users.aspx";

    private readonly UsersControllerSearch _usersControllerSearch;

    public UsersController(UsersControllerSearch usersControllerSearch){
        _usersControllerSearch = usersControllerSearch;
    }

    public ActionResult Search(string searchTerm, SetsModel model)
    {
        _sessionUiData.SearchSpecUser.SearchTearm = model.SearchTerm = searchTerm;
        return Users(null, model);
    }

    [SetMenu(MenuEntry.Users)]
    public ActionResult Users(int? page, SetsModel model)
    {
        _sessionUiData.SearchSpecUser.PageSize = 10;
        if (page.HasValue) _sessionUiData.SearchSpecUser.CurrentPage = page.Value;

        var users = _usersControllerSearch.Run();

        return View(_viewLocation, new UsersModel(users, _sessionUser));
    }       
}