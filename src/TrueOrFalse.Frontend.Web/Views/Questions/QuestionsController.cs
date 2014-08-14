using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

namespace TrueOrFalse
{

    public class QuestionsController : BaseController
    {
        private readonly QuestionRepository _questionRepository;
        private readonly QuestionsControllerSearch _questionsControllerSearch;
        private readonly QuestionsControllerUtil _util;

        public QuestionsController(
            QuestionRepository questionRepository,
            QuestionsControllerSearch questionsControllerSearch)
        {
            _questionRepository = questionRepository;
            _questionsControllerSearch = questionsControllerSearch;

            _util = new QuestionsControllerUtil(questionsControllerSearch);
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult Questions(int? page, QuestionsModel model, string orderBy)
        {
            _util.SetSearchSpecVars(_sessionUiData.SearchSpecQuestionAll, page, model, orderBy);

            return View("Questions",
                new QuestionsModel(
                    _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionAll), 
                    _sessionUiData.SearchSpecQuestionAll,
                    SearchTab.All));
        }

        public ActionResult QuestionsSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionAll, model, searchTerm);
            return Questions(page, model, orderBy);
        }

        public JsonResult QuestionsSearchApi(string searchTerm)
        {
            var model = new QuestionsModel();
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionAll, model, searchTerm);

            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionAll, SearchTab.All, ControllerContext);
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult QuestionsMine(int? page, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn){
                return View("Questions",
                    new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), SearchTab.Mine));
            }

            return View("Questions", _util.GetQuestionsModel(page, model, orderBy, _sessionUiData.SearchSpecQuestionMine, SearchTab.Mine));
        }

        public ActionResult QuestionsMineSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);
            return QuestionsMine(page, model, orderBy);
        }

        public JsonResult QuestionsMineSearchApi(string searchTerm)
        {
            var model = new QuestionsModel();
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);

            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionMine, SearchTab.Mine, ControllerContext);
        }

        [SetMenu(MenuEntry.Questions)]
        public ActionResult QuestionsWish(int? page, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn){
                return View("Questions",
                    new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), SearchTab.Wish));
            }

            return View("Questions", _util.GetQuestionsModel(page, model, orderBy, _sessionUiData.SearchSpecQuestionWish, SearchTab.Wish));
        }

        public ActionResult QuestionsWishSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionWish, model, searchTerm);
            return QuestionsWish(page, model, orderBy);
        }

        public JsonResult QuestionsWishSearchApi(string searchTerm)
        {
            var model = new QuestionsModel();
            _util.SetSearchTerm(_sessionUiData.SearchSpecQuestionWish, model, searchTerm);

            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionWish, SearchTab.Wish, ControllerContext);
        }

        [HttpPost]
        public JsonResult DeleteDetails(int questionId)
        {
            var question = _questionRepository.GetById(questionId);

            return new JsonResult{
                Data = new{
                    questionTitle = question.Text.WordWrap(50),
                    totalAnswers = question.TotalAnswers()
                }
            };
        }

        [HttpPost]
        public EmptyResult Delete(int questionId)
        {
            Sl.Resolve<QuestionDeleter>().Run(questionId);
            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult GetQuestionSets(string filter)
        {
            var searchSpec = new SetSearchSpec{PageSize = 12};
            var searchResult = Resolve<SearchSets>().Run(
                filter, searchSpec, _sessionUser.User.Id, startsWithSearch:true);
            var questionSets = Resolve<SetRepository>().GetByIds(searchResult.SetIds.ToArray());

            return new JsonResult{
                Data = new{
                   Sets = questionSets.Select(s => new{Id = s.Id, Name = s.Name, QuestionCount = s.QuestionsInSet.Count}),
                   Total = searchResult.Count
                }
            };
        }

        [HttpPost]
        public JsonResult AddToQuestionSet()
        {
            var parts = Request.Form[0].Split(':');
            var questionIds = parts[0].Split(',').Select(id => Convert.ToInt32(id)).ToArray();
            var questionSetId = Convert.ToInt32(parts[1]);
        
            var result = Resolve<AddToSet>().Run(questionIds, questionSetId);
        
            return new JsonResult{ Data = new{
                QuestionsAddedCount = result.AmountAddedQuestions,
                QuestionAlreadyInSet = result.AmountOfQuestionsAlreadyInSet
            }};
        }
    }

    public class QuestionsControllerUtil : BaseUtil
    {
        private readonly QuestionsControllerSearch _ctlSearch;

        public QuestionsControllerUtil(QuestionsControllerSearch ctlSearch){
            _ctlSearch = ctlSearch;
        }

        public QuestionsModel GetQuestionsModel(
            int? page,
            QuestionsModel model,
            string orderBy,
            QuestionSearchSpec searchSpec,
            SearchTab searchTab)
        {
            SetSearchSpecVars(searchSpec, page, model, orderBy);

            if (searchTab == SearchTab.Mine){
                searchSpec.Filter.CreatorId = _sessionUser.UserId;
            }else if (searchTab == SearchTab.Wish){
                searchSpec.Filter.ValuatorId = _sessionUser.UserId;
            }

            var questionsModel = new QuestionsModel(_ctlSearch.Run(searchSpec), searchSpec, searchTab);

            return questionsModel;
        }

        public JsonResult SearchApi(
            string searchTerm,
            QuestionSearchSpec searchSpec,
            SearchTab searchTab, 
            ControllerContext controllerContext)
        {
            var model = new QuestionsModel();
            SetSearchTerm(searchSpec, model, searchTerm);

            var totalInSystem = 0;
            switch (searchTab){
                case SearchTab.All: totalInSystem = R<GetTotalQuestionCount>().Run(); break;
                case SearchTab.Mine: totalInSystem = R<GetTotalQuestionCount>().Run(_sessionUser.UserId); break;
                case SearchTab.Wish: totalInSystem = R<GetWishQuestionCountCached>().Run(_sessionUser.UserId); break;
            }

            return new JsonResult
            {
                Data = new
                {
                    Html = ViewRenderer.RenderPartialView(
                        "QuestionsSearchResult",
                        new QuestionsSearchResultModel(
                            GetQuestionsModel(
                                searchSpec.CurrentPage, model,
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

        public void SetSearchSpecVars(QuestionSearchSpec searchSpec, int? page, QuestionsModel model,
            string orderBy, string defaultOrder = "byRelevance")
        {
            searchSpec.PageSize = 20;

            if (page.HasValue)
                searchSpec.CurrentPage = page.Value;

            SetOrderBy(searchSpec, orderBy, defaultOrder);
        }

        public void SetSearchTerm(QuestionSearchSpec searchSpec, QuestionsModel model, string searchTerm)
        {
            if (searchSpec.Filter.SearchTerm != searchTerm)
                searchSpec.CurrentPage = 1;

            if (!String.IsNullOrEmpty(searchTerm))
                searchTerm = searchTerm
                    .Replace("Kat__", "Kat:\"")
                    .Replace("Ersteller__", "Ersteller:\"")
                    .Replace("__", "\"")
                    .Replace("___", ":")
                    .Replace("_and_", "&");

            searchSpec.Filter.SearchTerm = model.SearchTerm = searchTerm;
        }

        public void SetOrderBy(QuestionSearchSpec searchSpec, string orderByCommand, string defaultOrder)
        {
            if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
                orderByCommand = defaultOrder;

            if (orderByCommand == "byRelevance") searchSpec.OrderBy.OrderByPersonalRelevance.Desc();
            else if (orderByCommand == "byQuality") searchSpec.OrderBy.OrderByQuality.Desc();
            else if (orderByCommand == "byDateCreated") searchSpec.OrderBy.OrderByCreationDate.Desc();
            else if (orderByCommand == "byViews") searchSpec.OrderBy.OrderByViews.Desc();
        }
    }
}