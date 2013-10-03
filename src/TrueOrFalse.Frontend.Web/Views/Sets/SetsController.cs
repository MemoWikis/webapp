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
    private readonly SetValuationRepository _setValuationRepository;
    private readonly SetsControllerSearch _setsControllerSearch;

    public SetsController(
        SetRepository setRepo, 
        SetValuationRepository setValuationRepository,
        SetsControllerSearch setsControllerSearch)
    {
        _setRepo = setRepo;
        _setValuationRepository = setValuationRepository;
        _setsControllerSearch = setsControllerSearch;
    }

    public ActionResult SetsWishSearch(string searchTerm, SetsModel model)
    {
        _sessionUiData.SearchSpecSetWish.SearchTearm = model.SearchTerm = searchTerm;
        return SetsWish(null, model);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsWish(int? page, SetsModel model)
    {
        if (page.HasValue)
            _sessionUiData.SearchSpecSetWish.CurrentPage = page.Value;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetWish);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetAll, GetValuations(sets), isTabWishActice: true));
    }

    public ActionResult SetsMineSearch(string searchTerm, SetsModel model)
    {
        _sessionUiData.SearchSpecSetMine.SearchTearm = model.SearchTerm = searchTerm;
        return SetsMine(null, model);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsMine(int? page, SetsModel model)
    {
        if (page.HasValue)
            _sessionUiData.SearchSpecSetMine.CurrentPage = page.Value;

        _sessionUiData.SearchSpecSetMine.Filter.CreatorId.EqualTo(_sessionUser.User.Id);

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetMine);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetMine, GetValuations(sets), isTabMineActive: true));
    }

    public ActionResult SetsSearch(string searchTerm, SetsModel model)
    {
        _sessionUiData.SearchSpecSetAll.SearchTearm = model.SearchTerm = searchTerm;
        return Sets(null, model);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult Sets(int? page, SetsModel model)
    {
        if (page.HasValue) 
            _sessionUiData.SearchSpecSetAll.CurrentPage = page.Value;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetAll);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetAll, GetValuations(sets), isTabAllActive: true));
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

    [HttpPost]
    public JsonResult SaveRelevancePersonal(int id, int newValue)
    {
        var oldKnowledgeCount = Sl.Resolve<GetWishSetCount>().Run(_sessionUser.User.Id);

        Sl.Resolve<UpdateSetsTotals>().UpdateRelevancePersonal(id, _sessionUser.User.Id, newValue);
        var totals = Sl.Resolve<GetSetTotal>().RunForRelevancePersonal(id);

        var newKnowledgeCount = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, forceReload: true);

        return new JsonResult
        {
            Data = new
            {
                totalValuations = totals.Count,
                totalAverage = Math.Round(totals.Avg / 10d, 1),
                totalWishKnowledgeCount = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id, forceReload: true),
                totalWishKnowledgeCountChange = oldKnowledgeCount != newKnowledgeCount
            }
        };
    }

    public IList<SetValuation> GetValuations(IEnumerable<Set> sets)
    {
        return _setValuationRepository.GetBy(sets.GetIds(), _sessionUser.User.Id);
    }
}