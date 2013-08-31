using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
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
    public ActionResult SetsWish(int? page, SetsModel model)
    {
        var questionSets = _setsControllerSearch.Run();
        return View(_viewLocation, new SetsModel(questionSets) { ActiveTabWish = true});
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsMine(int? page, SetsModel model)
    {
        var questionSets = _setsControllerSearch.Run();
        return View(_viewLocation, new SetsModel(questionSets) { ActiveTabMine = true });
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult Sets(int? page, SetsModel model)
    {
        _sessionUiData.SearchSpecSet.PageSize = 10;
        if (page.HasValue) 
            _sessionUiData.SearchSpecSet.CurrentPage = page.Value;

        var questionSets = _setsControllerSearch.Run();

        return View(_viewLocation, new SetsModel(questionSets) { ActiveTabAll = true });
    }

    [HttpPost]
    public JsonResult DeleteDetails(int setId)
    {
        var question = _setRepo.GetById(setId);

        return new JsonResult{
            Data = new{
                questionTitle = question.Text.WordWrap(50),
            }
        };
    }

    [HttpPost]
    public EmptyResult Delete(int setId)
    {
        Sl.Resolve<SetDeleter>().Run(setId);
        return new EmptyResult();
    }
}