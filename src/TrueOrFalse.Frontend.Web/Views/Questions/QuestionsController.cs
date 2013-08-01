using System;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;

public class QuestionsController : BaseController
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionValuationRepository _questionValuationRepository;
    private readonly QuestionsControllerSearch _questionSearchPage;
    private readonly TotalsPersUserLoader _totalsPerUserLoader;
    private readonly UserRepository _userRepository;

    public QuestionsController(QuestionRepository questionRepository,
                               QuestionValuationRepository questionValuationRepository,
                               QuestionsControllerSearch questionSearchPage,
                               TotalsPersUserLoader totalsPerUserLoader, 
                               UserRepository userRepository)
    {
        _questionRepository = questionRepository;
        _questionValuationRepository = questionValuationRepository;
        _questionSearchPage = questionSearchPage;
        _totalsPerUserLoader = totalsPerUserLoader;
        _userRepository = userRepository;
    }

    public ActionResult OrderByPersonalRelevance(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestion.OrderBy.OrderByPersonalRelevance.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByQuality(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestion.OrderBy.OrderByQuality.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByCreationDate(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestion.OrderBy.OrderByCreationDate.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByViews(int? page, QuestionsModel model){
        _sessionUiData.SearchSpecQuestion.OrderBy.OrderByViews.Desc();
        return Questions(page, model);
    }

    public ActionResult QuestionSearch(string searchTerm, QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestion.SearchTearm = model.SearchTerm = searchTerm;
        return Questions(null, model);
    }

    [SetMenu(MenuEntry.Questions)]
    public ActionResult Questions(int? page, QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestion.PageSize = 10;

        _sessionUiData.SearchSpecQuestion.SetFilterByMe(model.FilterByMe);
        _sessionUiData.SearchSpecQuestion.SetFilterByAll(model.FilterByAll);
        _sessionUiData.SearchSpecQuestion.AddFilterByUser(model.AddFilterUser);
        _sessionUiData.SearchSpecQuestion.DelFilterByUser(model.DelFilterUser);

        if (!_sessionUiData.SearchSpecQuestion.OrderBy.IsSet())
            _sessionUiData.SearchSpecQuestion.OrderBy.OrderByPersonalRelevance.Desc();

        if (page.HasValue) _sessionUiData.SearchSpecQuestion.CurrentPage = page.Value;

        var questions = _questionSearchPage.Run();
        var totalsForCurrentUser = _totalsPerUserLoader.Run(_sessionUser.User.Id, questions);
        var questionValutionsForCurrentUser = _questionValuationRepository.GetBy(questions.GetIds(), _sessionUser.User.Id);

        return View("Questions",
                    new QuestionsModel(
                        questions,
                        totalsForCurrentUser,
                        questionValutionsForCurrentUser,
                        _sessionUiData.SearchSpecQuestion,
                        _sessionUser.User.Id)
                    {
                        Pager = new PagerModel(_sessionUiData.SearchSpecQuestion),
                        FilterByMe = _sessionUiData.SearchSpecQuestion.FilterByMe,
                        FilterByAll = _sessionUiData.SearchSpecQuestion.FilterByAll,
                        FilterByUsers = _userRepository.GetByIds(_sessionUiData.SearchSpecQuestion.FilterByUsers.ToArray()).ToDictionary(user => user.Id, user => user.Name),
                        TotalQuestionsInSystem = Sl.Resolve<GetTotalQuestionCount>().Run(),
                        TotalWishKnowledge = Sl.Resolve<GetWishKnowledgeCountCached>().Run(_sessionUser.User.Id)
                    }
            );
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
    public JsonResult GetQuestionSets()
    {
        var searchSpec = new SetSearchSpec{PageSize = 7};
        var questionSets = Resolve<SetRepository>().GetBy(searchSpec);

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