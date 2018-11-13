using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Seedworks.Lib;

public class SetsController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Sets.aspx";

    private readonly SetRepo _setRepo;
    private readonly SetsControllerSearch _setsControllerSearch;
    private readonly SetsControllerUtil _util;

    public SetsController(SetRepo setRepo)
    {
        _setRepo = setRepo;
        _setsControllerSearch = new SetsControllerSearch();

        _util = new SetsControllerUtil(_setsControllerSearch);
    }

    public ActionResult SetsWishSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetWish, model, searchTerm);
        return SetsWish(page, model, orderBy);
    }

    public ActionResult SetsWishSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetWish, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetWish, SearchTabType.Wish, ControllerContext);
    }

    [SetMainMenu(MainMenuEntry.QuestionSet)]
    [SetThemeMenu]
    public ActionResult SetsWish(int? page, SetsModel model, string orderBy)
    {
        if (!_sessionUser.IsLoggedIn)
            return View(_viewLocation,
                new SetsModel(new List<Set>(), new SetSearchSpec(), SearchTabType.Wish));

        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetWish, page, orderBy);

        _sessionUiData.SearchSpecSetWish.Filter.ValuatorId = _sessionUser.UserId;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetWish);
        return View(_viewLocation, 
            new SetsModel(sets, _sessionUiData.SearchSpecSetWish, SearchTabType.Wish));
    }

    public ActionResult SetsMineSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetMine, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetMine, SearchTabType.Mine, ControllerContext);
    }

    public ActionResult SetsMineSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetMine, model, searchTerm);
        return SetsMine(page, model, orderBy);
    }

    [SetMainMenu(MainMenuEntry.QuestionSet)]
    [SetThemeMenu]
    public ActionResult SetsMine(int? page, SetsModel model, string orderBy)
    {
        if (!_sessionUser.IsLoggedIn)
            return View(_viewLocation,
                new SetsModel(new List<Set>(), new SetSearchSpec(), SearchTabType.Mine));

        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetMine, page, orderBy);

        _sessionUiData.SearchSpecSetMine.Filter.CreatorId = _sessionUser.UserId;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetMine);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetMine, SearchTabType.Mine));
    }

    public ActionResult SetsSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetsAll, model, searchTerm);
        return Sets(page, model, orderBy);
    }

    public ActionResult SetsSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetsAll, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetsAll, SearchTabType.All, ControllerContext);
    }

    [SetMainMenu(MainMenuEntry.QuestionSet)]
    [SetThemeMenu]
    public ActionResult Sets(int? page, SetsModel model, string orderBy)
    {
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetsAll, page, orderBy);

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetsAll);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetsAll, SearchTabType.All));
    }

    [HttpPost]
    public JsonResult DeleteDetails(int setId)
    {
        var set = _setRepo.GetById(setId);

        var canBeDeleted = SetDeleter.CanBeDeleted(set.Creator.Id, setId);

        return new JsonResult
        {
            Data = new
            {
                setTitle = set.Name.TruncateAtWord(90),
                canNotBeDeleted = !canBeDeleted.Yes,
                canNotBeDeletedReason = canBeDeleted.IfNot_Reason
            }
        };
    }

    [HttpPost]
    public EmptyResult Delete(int setId)
    {
        SetDeleter.Run(setId);
        return new EmptyResult();
    }

    public class SetsControllerUtil : BaseUtil
    {
        private readonly SetsControllerSearch _setsControllerSearch;

        public SetsControllerUtil(SetsControllerSearch setsControllerSearch)
        {
            _setsControllerSearch = setsControllerSearch;
        }

        public SetsModel GetSetsModel(
            int? page,
            SetsModel model,
            string orderBy,
            SetSearchSpec searchSpec,
            SearchTabType searchTab)
        {
            SetSearchSpecVars(searchSpec, page, orderBy);

            if (searchTab == SearchTabType.Mine)
                searchSpec.Filter.CreatorId = _sessionUser.UserId;
            else if (searchTab == SearchTabType.Wish)
                searchSpec.Filter.ValuatorId = _sessionUser.UserId;

            var questionsModel = new SetsModel(_setsControllerSearch.Run(searchSpec), searchSpec, searchTab);

            return questionsModel;
        }

        public JsonResult SearchApi(
            string searchTerm,
            SetSearchSpec searchSpec,
            SearchTabType searchTab,
            ControllerContext controllerContext)
        {
            var model = new SetsModel();
            SetSearchFilter(searchSpec, model, searchTerm);

            var totalInSystem = 0;
            switch (searchTab)
            {
                case SearchTabType.All: totalInSystem = R<GetTotalSetCount>().Run(); break;
                case SearchTabType.Mine: totalInSystem = R<GetTotalSetCount>().Run(_sessionUser.UserId); break;
                case SearchTabType.Wish: totalInSystem = R<GetWishSetCount>().Run(_sessionUser.UserId); break;
            }

            return new JsonResult
            {
                Data = new
                {
                    Html = ViewRenderer.RenderPartialView(
                        "SetsSearchResult",
                        new SetsSearchResultModel(
                            GetSetsModel(
                                searchSpec.CurrentPage, 
                                model,
                                "",
                                searchSpec,
                                searchTab
                                )),
                        controllerContext),
                    TotalInResult = searchSpec.TotalItems,
                    TotalInSystem = totalInSystem,
                    Tab = searchTab.ToString()
                },
            };
        }

        public void SetSearchSpecVars(SetSearchSpec searchSpec, int? page, string orderByCommand)
        {
            if (page.HasValue)
                searchSpec.CurrentPage = page.Value;

            SetSetsOrderBy(searchSpec, orderByCommand);
        }

        private void SetSetsOrderBy(SetSearchSpec searchSpec, string orderByCommand)
        {
            if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
                orderByCommand = "byBestMatch";

            if (orderByCommand == "byBestMatch") searchSpec.OrderBy.BestMatch.Desc();
            else if (orderByCommand == "byValuationsCount") searchSpec.OrderBy.ValuationsCount.Desc();
            else if (orderByCommand == "byCreationDate") searchSpec.OrderBy.CreationDate.Desc();
            else if (orderByCommand == "byValuationsAvg") searchSpec.OrderBy.ValuationsAvg.Desc();
        }

        public void SetSearchFilter(
            SetSearchSpec searchSpec,
            SetsModel model,
            string searchTerm)
        {
            if (searchSpec.SearchTerm != searchTerm)
                searchSpec.CurrentPage = 1;

            searchSpec.SearchTerm = model.SearchTerm = searchTerm;
        }
    }

}