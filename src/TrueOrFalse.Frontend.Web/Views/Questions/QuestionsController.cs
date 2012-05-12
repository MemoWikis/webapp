using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class QuestionsController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;

    public QuestionsController (QuestionRepository questionRepository,
                                UserRepository userRepository, 
                                SessionUiData sessionUiData)
    {
        _questionRepository = questionRepository;
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
    }

    public ActionResult Questions(int? page, QuestionsModel model)
    {
        _sessionUiData.QuestionSearchSpec.PageSize = 10;

        _sessionUiData.QuestionSearchSpec.SetFilterByMe(model.FilterByMe);
        _sessionUiData.QuestionSearchSpec.SetFilterByAll(model.FilterByAll);
        _sessionUiData.QuestionSearchSpec.AddFilterByUser(model.AddFilterUser);

        model.FilterByMe = _sessionUiData.QuestionSearchSpec.FilterByMe;
        model.FilterByAll = _sessionUiData.QuestionSearchSpec.FilterByAll;
        model.FilterByUsers =
            _userRepository.GetByIds(_sessionUiData.QuestionSearchSpec.FilterByUsers.ToArray())
                .ToDictionary(user => user.Id, user => user.Name);

        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View("Questions",
                    new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec), _sessionUiData.QuestionSearchSpec)
                    {Pager = new PagerModel(_sessionUiData.QuestionSearchSpec),
                     FilterByMe = model.FilterByMe,
                     FilterByAll = model.FilterByAll,
                     FilterByUsers =  model.FilterByUsers});
    }

    [HttpPost]
    public JsonResult GetQuestionDeleteDetails(int questionId)
    {
        var question = _questionRepository.GetById(questionId);

        return new JsonResult{
            Data = new
                       {
                           questionTitle = question.Text.WordWrap(50),
                           totalAnswers = question.TotalAnswers()
                       }
        };
    }

}
