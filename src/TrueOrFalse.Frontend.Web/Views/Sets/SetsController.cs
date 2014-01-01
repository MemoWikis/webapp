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

    public ActionResult SetsWishSearch(string searchTerm, SetsModel model, int? page)
    {
        if (_sessionUiData.SearchSpecSetWish.SearchTerm != searchTerm)
            _sessionUiData.SearchSpecSetWish.CurrentPage = 1;

        _sessionUiData.SearchSpecSetWish.SearchTerm = model.SearchTerm = searchTerm;
        return SetsWish(page, model);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsWish(int? page, SetsModel model)
    {
        if (page.HasValue)
            _sessionUiData.SearchSpecSetWish.CurrentPage = page.Value;

        _sessionUiData.SearchSpecSetWish.Filter.ValuatorId = _sessionUser.User.Id;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetWish);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetWish, GetValuations(sets), isTabWishActice: true));
    }

    public ActionResult SetsMineSearch(string searchTerm, SetsModel model, int? page)
    {
        if (_sessionUiData.SearchSpecSetMine.SearchTerm != searchTerm)
            _sessionUiData.SearchSpecSetMine.CurrentPage = 1;

        _sessionUiData.SearchSpecSetMine.SearchTerm = model.SearchTerm = searchTerm;
        return SetsMine(page, model);
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

    public ActionResult SetsSearch(string searchTerm, SetsModel model, int? page)
    {
        if (_sessionUiData.SearchSpecSetAll.SearchTerm != searchTerm)
            _sessionUiData.SearchSpecSetAll.CurrentPage = 1;

        _sessionUiData.SearchSpecSetAll.SearchTerm = model.SearchTerm = searchTerm;
        return Sets(page, model);
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
        var set = _setRepo.GetById(setId);

        return new JsonResult{
            Data = new{
                setTitle = set.Name.WordWrap(50),
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

        Sl.Resolve<UpdateSetsTotals>().UpdateRelevancePersonal(id, _sessionUser.User, newValue);
        var totals = Sl.Resolve<GetSetTotal>().RunForRelevancePersonal(id);

        var newKnowledgeCount = Sl.Resolve<GetWishSetCount>().Run(_sessionUser.User.Id);

        return new JsonResult
        {
            Data = new
            {
                totalValuations = totals.Count,
                totalAverage = Math.Round(totals.Avg / 10d, 1),
                totalWishKnowledgeCount = newKnowledgeCount,
                totalWishKnowledgeCountChange = oldKnowledgeCount != newKnowledgeCount
            }
        };
    }

    public IList<SetValuation> GetValuations(IEnumerable<Set> sets)
    {
        return _setValuationRepository.GetBy(sets.GetIds(), _sessionUser.User.Id);
    }
}