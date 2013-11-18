using System;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Search;

public class QuestionsController : BaseController
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionsControllerSearch _questionsControllerSearch;

    public QuestionsController(QuestionRepository questionRepository,
                               QuestionsControllerSearch questionsControllerSearch)
    {
        _questionRepository = questionRepository;
        _questionsControllerSearch = questionsControllerSearch;
    }

    public ActionResult OrderByPersonalRelevance(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByPersonalRelevance.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByQuality(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByQuality.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByCreationDate(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByCreationDate.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByViews(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByViews.Desc();
        return Questions(page, model);
    }

    public ActionResult QuestionSearch(string searchTerm, QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestionAll.SearchTearm = model.SearchTerm = searchTerm;
        return Questions(null, model);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult Questions(int? page, QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestionAll.PageSize = 10;

        if (page.HasValue) 
            _sessionUiData.SearchSpecQuestionAll.CurrentPage = page.Value;

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(model), 
                _sessionUiData.SearchSpecQuestionAll, 
                _sessionUser.User.Id,
                isTabAllActive: true));
    }

    public ActionResult QuestionsMineSearch(string searchTerm, QuestionsModel model)
    {
        _sessionUiData.SearchSpecSetMine.SearchTearm = model.SearchTerm = searchTerm;
        return QuestionsMine(null, model);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult QuestionsMine(int? page, QuestionsModel model)
    {
        if (page.HasValue)
            _sessionUiData.SearchSpecQuestionMine.CurrentPage = page.Value;

        _sessionUiData.SearchSpecQuestionMine.Filter.CreatorId = _sessionUser.User.Id;

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(model),
                _sessionUiData.SearchSpecQuestionMine, 
                _sessionUser.User.Id,
                isTabMineActive: true));
    }

    public ActionResult QuestionsWishSearch(string searchTerm, QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestionWish.SearchTearm = model.SearchTerm = searchTerm;
        return QuestionsWish(null, model);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult QuestionsWish(int? page, QuestionsModel model)
    {
        if (page.HasValue)
            _sessionUiData.SearchSpecQuestionWish.CurrentPage = page.Value;

        _sessionUiData.SearchSpecQuestionWish.Filter.ValuatorId = _sessionUser.User.Id;

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(model),
                _sessionUiData.SearchSpecQuestionWish,
                _sessionUser.User.Id,
                isTabWishActice: true));
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
        var questionSets = Resolve<SetRepository>()
            .GetByIds(Resolve<SearchSets>().Run(filter, searchSpec, _sessionUser.User.Id).SetIds.ToArray());

        return new JsonResult{
            Data = new{
               Sets = questionSets.Select(s => new{Id = s.Id, Name = s.Name})
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