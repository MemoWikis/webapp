using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;

public class SetsController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Sets.aspx";

    private readonly SetRepository _setRepo;
    private readonly SetsControllerSearch _setsControllerSearch;
    private readonly SetsControllerUtil _util;

    public SetsController(
        SetRepository setRepo, 
        SetsControllerSearch setsControllerSearch)
    {
        _setRepo = setRepo;
        _setsControllerSearch = setsControllerSearch;

        _util = new SetsControllerUtil(setsControllerSearch);
    }

    public ActionResult SetsWishSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetWish, model, searchTerm);
        return SetsWish(page, model, orderBy);
    }

    public ActionResult SetsWishSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetWish, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetWish, SearchTab.Wish, ControllerContext);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsWish(int? page, SetsModel model, string orderBy)
    {
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetWish, page, orderBy);

        _sessionUiData.SearchSpecSetWish.Filter.ValuatorId = _sessionUser.UserId;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetWish);
        return View(_viewLocation, 
            new SetsModel(sets, _sessionUiData.SearchSpecSetWish, SearchTab.Wish));
    }

    public ActionResult SetsMineSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetMine, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetMine, SearchTab.Mine, ControllerContext);
    }

    public ActionResult SetsMineSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetMine, model, searchTerm);
        return SetsMine(page, model, orderBy);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult SetsMine(int? page, SetsModel model, string orderBy)
    {
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetMine, page, orderBy);

        _sessionUiData.SearchSpecSetMine.Filter.CreatorId = _sessionUser.UserId;

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetMine);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetMine, SearchTab.Mine));
    }

    public ActionResult SetsSearch(string searchTerm, SetsModel model, int? page, string orderBy)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetsAll, model, searchTerm);
        return Sets(page, model, orderBy);
    }

    public JsonResult SetsSearchApi(string searchTerm)
    {
        _util.SetSearchFilter(_sessionUiData.SearchSpecSetsAll, new SetsModel(), searchTerm);
        return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecSetsAll, SearchTab.All, ControllerContext);
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult Sets(int? page, SetsModel model, string orderBy)
    {
        _util.SetSearchSpecVars(_sessionUiData.SearchSpecSetsAll, page, orderBy);

        var sets = _setsControllerSearch.Run(_sessionUiData.SearchSpecSetsAll);
        return View(_viewLocation, new SetsModel(sets, _sessionUiData.SearchSpecSetsAll, SearchTab.All));
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
            SearchTab searchTab)
        {
            SetSearchSpecVars(searchSpec, page, orderBy);

            if (searchTab == SearchTab.Mine)
                searchSpec.Filter.CreatorId = _sessionUser.UserId;
            else if (searchTab == SearchTab.Wish)
                searchSpec.Filter.ValuatorId = _sessionUser.UserId;

            var questionsModel = new SetsModel(_setsControllerSearch.Run(searchSpec), searchSpec, searchTab);

            return questionsModel;
        }

        public JsonResult SearchApi(
            string searchTerm,
            SetSearchSpec searchSpec,
            SearchTab searchTab,
            ControllerContext controllerContext)
        {
            var model = new SetsModel();
            SetSearchFilter(searchSpec, model, searchTerm);

            var totalInSystem = 0;
            switch (searchTab)
            {
                case SearchTab.All: totalInSystem = R<GetTotalSetCount>().Run(); break;
                case SearchTab.Mine: totalInSystem = R<GetTotalSetCount>().Run(_sessionUser.UserId); break;
                case SearchTab.Wish: totalInSystem = R<GetWishSetCount>().Run(_sessionUser.UserId); break;
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
                orderByCommand = "byValuationsCount";

            if (orderByCommand == "byValuationsCount") searchSpec.OrderBy.ValuationsCount.Desc();
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