using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Criterion;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
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

    public ActionResult QuestionSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
    {
        SetSearchSearchTerm(_sessionUiData.SearchSpecQuestionAll, model, searchTerm);
        return Questions(page, model, orderBy);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult Questions(int? page, QuestionsModel model, string orderBy)
    {
        SetSearchSpecVars(_sessionUiData.SearchSpecQuestionAll, page, model, orderBy);

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionAll), 
                _sessionUiData.SearchSpecQuestionAll, 
                _sessionUser.User.Id,
                isTabAllActive: true));
    }

    public ActionResult QuestionsMineSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
    {
        SetSearchSearchTerm(_sessionUiData.SearchSpecQuestionMine, model, searchTerm);
        return QuestionsMine(page, model, orderBy);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult QuestionsMine(int? page, QuestionsModel model, string orderBy)
    {
        SetSearchSpecVars(_sessionUiData.SearchSpecQuestionMine, page, model, orderBy);
        _sessionUiData.SearchSpecQuestionMine.Filter.CreatorId = _sessionUser.User.Id;

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionMine),
                _sessionUiData.SearchSpecQuestionMine, 
                _sessionUser.User.Id,
                isTabMineActive: true));
    }

    public ActionResult QuestionsWishSearch(string searchTerm, QuestionsModel model, int? page, string orderBy)
    {
        SetSearchSearchTerm(_sessionUiData.SearchSpecQuestionWish, model, searchTerm);
        return QuestionsWish(page, model, orderBy);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult QuestionsWish(int? page, QuestionsModel model, string orderBy)
    {
        SetSearchSpecVars(_sessionUiData.SearchSpecQuestionWish, page, model, orderBy);
        _sessionUiData.SearchSpecQuestionWish.Filter.ValuatorId = _sessionUser.User.Id;

        return View("Questions",
            new QuestionsModel(
                _questionsControllerSearch.Run(_sessionUiData.SearchSpecQuestionWish),
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

    public void SetSearchSearchTerm(QuestionSearchSpec searchSpec, QuestionsModel model, string searchTerm)
    {
        if (searchSpec.Filter.SearchTerm != searchTerm)
            searchSpec.CurrentPage = 1;

        searchSpec.Filter.SearchTerm = model.SearchTerm = searchTerm;        
    }

    public void SetSearchSpecVars(QuestionSearchSpec searchSpec, int? page, QuestionsModel model, string orderBy, string defaultOrder = "byViews")
    {
        searchSpec.PageSize = 10;

        if (page.HasValue)
            searchSpec.CurrentPage = page.Value;

        SetOrderBy(searchSpec, orderBy, defaultOrder);        
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