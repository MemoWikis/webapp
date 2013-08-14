using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class SetsController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Sets.aspx";

    private readonly SetRepository _setRepo;
    private readonly SetsControllerSearch _setsControllerSearch;

    public SetsController(
        SetRepository setRepo, 
        SetsControllerSearch setsControllerSearch)
    {
        _setRepo = setRepo;
        _setsControllerSearch = setsControllerSearch;
    }

    public ActionResult Search(string searchTerm, SetsModel model)
    {
        _sessionUiData.SearchSpecSet.SearchTearm = model.SearchTerm = searchTerm;
        return Sets(null, model);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult Sets(int? page, SetsModel model)
    {
        _sessionUiData.SearchSpecSet.PageSize = 10;
        if (page.HasValue) _sessionUiData.SearchSpecSet.CurrentPage = page.Value;

        var questionSets = _setsControllerSearch.Run();

        return View(_viewLocation, new SetsModel(questionSets));
    }       
}