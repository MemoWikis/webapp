using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class QuestionsController : BaseController
    {
        private readonly QuestionRepo _questionRepo;
        private readonly QuestionsControllerSearch _questionsControllerSearch;
        private readonly QuestionsControllerUtil _util;
         
        public QuestionsController(QuestionRepo questionRepo)
        {
            _questionRepo = questionRepo;
            _questionsControllerSearch = new QuestionsControllerSearch();

            _util = new QuestionsControllerUtil(_questionsControllerSearch);
        }

        [SetMainMenu(MainMenuEntry.Questions)]
        public ActionResult Questions(int? page, QuestionsModel model, string orderBy)
        {
            _util.SetSearchSpecVars(_sessionUiData.SearchSpecQuestionAll, page, model, orderBy);

            return View("Questions",
                new QuestionsModel(
                    _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionAll), 
                    _sessionUiData.SearchSpecQuestionAll,
                    SearchTabType.All));
        }

        [SetMainMenu(MainMenuEntry.Questions)]
        public ActionResult QuestionsSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionAll, model, searchTerm);
            return Questions(page, model, orderBy);
        }

        [SetMainMenu(MainMenuEntry.Questions)]
        public ActionResult QuestionsSearchCategoryFilter(string categoryName, int categoryId)
        {
            _sessionUiData.SearchSpecQuestionAll.Filter.Clear();
            _sessionUiData.SearchSpecQuestionAll.Filter.Categories.Add(categoryId);
            return Questions(1, new QuestionsModel(), null);
        }

        public JsonResult QuestionsSearchApi(string searchTerm, List<Int32> categories)
        {
            var model = new QuestionsModel();
            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionAll, model, searchTerm, categories ?? new List<int>());

            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionAll, SearchTabType.All, ControllerContext);
        }

        [SetMainMenu(MainMenuEntry.Questions)]
        public ActionResult QuestionsMine(int? page, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn){
                return View("Questions",
                    new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), SearchTabType.Mine));
            }

            return View("Questions", _util.GetQuestionsModel(page, model, orderBy, _sessionUiData.SearchSpecQuestionMine, SearchTabType.Mine));
        }

        public ActionResult QuestionsMineSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);
            return QuestionsMine(page, model, orderBy);
        }

        public JsonResult QuestionsMineSearchApi(string searchTerm, List<Int32> categories)
        {
            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionMine, new QuestionsModel(), searchTerm, categories ?? new List<int>());
            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionMine, SearchTabType.Mine, ControllerContext);
        }

        [SetMainMenu(MainMenuEntry.Questions)]
        public ActionResult QuestionsWish(int? page, string filter, QuestionsModel model, string orderBy)
        {
            if (!_sessionUser.IsLoggedIn)
                return View("Questions", new QuestionsModel(new List<Question>(), new QuestionSearchSpec(), SearchTabType.Wish));

            if (filter != null)
                _sessionUiData.SearchSpecQuestionWish.Filter.SetKnowledgeFilter(filter);

            return View("Questions", _util.GetQuestionsModel(page, model, orderBy, _sessionUiData.SearchSpecQuestionWish, SearchTabType.Wish));
        }

        public ActionResult QuestionsWishSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
        {
            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionWish, model, searchTerm);
            return QuestionsWish(page, null, model, orderBy);
        }

        public ActionResult QuestionsWishSearchCategoryFilter(string categoryName, int categoryId)
        {
            _sessionUiData.SearchSpecQuestionWish.Filter.Clear();
            _sessionUiData.SearchSpecQuestionWish.Filter.Categories.Add(categoryId);
            return QuestionsWish(1, null, new QuestionsModel(), null);
        }

        public JsonResult QuestionsWishSearchApi(string searchTerm, List<Int32> categories, string knowledgeFilter)
        {
            if (!String.IsNullOrEmpty(knowledgeFilter))
            {
                dynamic dKnowledgeFilter = JsonConvert.DeserializeObject(knowledgeFilter);

                var filter = _sessionUiData.SearchSpecQuestionWish.Filter;
                filter.Knowledge_Solid = dKnowledgeFilter.Knowledge_Solid;
                filter.Knowledge_ShouldConsolidate = dKnowledgeFilter.Knowledge_ShouldConsolidate;
                filter.Knowledge_ShouldLearn = dKnowledgeFilter.Knowledge_ShouldLearn;
                filter.Knowledge_None = dKnowledgeFilter.Knowledge_None;
            }

            _util.SetSearchFilter(_sessionUiData.SearchSpecQuestionWish, new QuestionsModel(), searchTerm, categories ?? new List<int>());
            return _util.SearchApi(searchTerm, _sessionUiData.SearchSpecQuestionWish, SearchTabType.Wish, ControllerContext);
        }

        [HttpPost]
        public JsonResult DeleteDetails(int questionId)
        {
            var question = _questionRepo.GetById(questionId);
            var userTiny = new UserTinyModel(question.Creator);
            var canBeDeleted = QuestionDelete.CanBeDeleted(userTiny.Id, questionId);

            return new JsonResult{
                Data = new{
                    questionTitle = question.Text.TruncateAtWord(90),
                    totalAnswers = question.TotalAnswers(),
                    canNotBeDeleted = !canBeDeleted.Yes,
                    canNotBeDeletedReason = canBeDeleted.IfNot_Reason
                }
            };
        }

        [HttpPost]
        public EmptyResult Delete(int questionId)
        {
            QuestionDelete.Run(questionId);
            return new EmptyResult();
        }

        [RedirectToErrorPage_IfNotLoggedIn]
        public ActionResult Restore(int questionId, int questionChangeId)
        {
            RestoreQuestion.Run(questionChangeId, this.User_());

            var question = Sl.QuestionRepo.GetById(questionId);
            return Redirect(Links.AnswerQuestion(question));
        }

        [HttpPost]
        public JsonResult LoadQuestion(int questionId)
        {
            var user = Sl.R<SessionUser>().User;
            var userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;
            var q = EntityCache.GetQuestionById(questionId);
            var question = new QuestionListJson.Question();
            question.Id = q.Id;
            question.Title = q.Text; 
            question.LinkToQuestion = Links.GetUrl(q);
            question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
            question.LinkToQuestion = Links.GetUrl(q);
            question.LinkToEditQuestion = Links.EditQuestion(q.Text, q.Id);
            question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
            question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
            question.CorrectnessProbability = q.CorrectnessProbability;
            question.Visibility = q.Visibility;

            var learningSession = LearningSessionCache.GetLearningSession();
            if (learningSession != null)
            {
                var steps = learningSession.Steps;
                var index = steps.IndexOf(s => s.Question.Id == q.Id);
                question.SessionIndex = index;
            }

            if (userQuestionValuation.ContainsKey(q.Id) && user != null)
            {
                question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
                question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
                question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
            }

            return Json(question);
        }

        [HttpPost]
        public JsonResult GetQuestionData(int questionId)
        {
            var question = EntityCache.GetQuestionById(questionId);
            var json = new JsonResult
            {
                Data = new
                {
                    SolutionType = (int)question.SolutionType,
                    Solution = question.Solution,
                    SolutionMetadataJson = question.SolutionMetadataJson,
                    Text = question.Text,
                    CategoryIds = question.Categories.Select(c => c.Id).ToList(),
                    DescriptionHtml = question.DescriptionHtml,
                    Categories = question.Categories.Select(c => new SearchApiController.MiniCategoryItem
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Url = Links.CategoryDetail(c.Name, c.Id),
                        QuestionCount = c.GetCountQuestionsAggregated(),
                        ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
                        IconHtml = SearchApiController.GetIconHtml(c),
                        MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category)).GetImageUrl(30, true, false, ImageType.Category).Url,
                        Visibility = (int)c.Visibility
                    })
                }
            };

            return json;
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
            SearchTabType searchTab)
        {
            SetSearchSpecVars(searchSpec, page, model, orderBy);

            if (searchTab == SearchTabType.Mine)
                searchSpec.Filter.CreatorId = _sessionUser.UserId;
            else if (searchTab == SearchTabType.Wish)
                searchSpec.Filter.ValuatorId = _sessionUser.UserId;

            var questionsModel = new QuestionsModel(_ctlSearch.Run(searchSpec), searchSpec, searchTab);

            return questionsModel;
        }

        public JsonResult SearchApi(
            string searchTerm,
            QuestionSearchSpec searchSpec,
            SearchTabType searchTab, 
            ControllerContext controllerContext)
        {
            var model = new QuestionsModel();
            SetSearchFilter(searchSpec, model, searchTerm);

            var totalInSystem = 0;
            switch (searchTab){
                case SearchTabType.All: totalInSystem = R<QuestionGetCount>().Run(); break;
                case SearchTabType.Mine: totalInSystem = R<QuestionGetCount>().Run(_sessionUser.UserId); break;
                case SearchTabType.Wish: totalInSystem = R<GetWishQuestionCountCached>().Run(_sessionUser.UserId); break;
            }

            return new JsonResult
            {
                Data = new
                {
                    Html = ViewRenderer.RenderPartialView(
                        "QuestionsSearchResult",
                        new QuestionsSearchResultModel(
                            GetQuestionsModel(
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

        public void SetSearchSpecVars(
            QuestionSearchSpec searchSpec, 
            int? page, 
            QuestionsModel model,
            string orderBy, 
            string defaultOrder = "byBestMatch")
        {
            searchSpec.PageSize = 20;

            if (page.HasValue)
                searchSpec.CurrentPage = page.Value;

            SetOrderBy(searchSpec, orderBy, defaultOrder);
        }

        public void SetSearchFilter(
            QuestionSearchSpec searchSpec, 
            QuestionsModel model, 
            string searchTerm, 
            List<int> categories = null)
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

            if (categories != null){
                searchSpec.Filter.Categories = categories;
            }
        }

        public void SetOrderBy(QuestionSearchSpec searchSpec, string orderByCommand, string defaultOrder)
        {
            if (searchSpec.OrderBy.Current == null && String.IsNullOrEmpty(orderByCommand))
                orderByCommand = defaultOrder;

            if (orderByCommand == "byBestMatch") searchSpec.OrderBy.BestMatch.Desc();
            else if (orderByCommand == "byRelevance") searchSpec.OrderBy.PersonalRelevance.Desc();
            else if (orderByCommand == "byQuality") searchSpec.OrderBy.OrderByQuality.Desc();
            else if (orderByCommand == "byDateCreated") searchSpec.OrderBy.CreationDate.Desc();
            else if (orderByCommand == "byViews") searchSpec.OrderBy.Views.Desc();
        }
    }

}