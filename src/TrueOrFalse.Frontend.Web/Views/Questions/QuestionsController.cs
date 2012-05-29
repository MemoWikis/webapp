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
    private readonly SessionUser _sessionUser;

    public QuestionsController (QuestionRepository questionRepository,
                                UserRepository userRepository, 
                                SessionUiData sessionUiData, 
                                SessionUser sessionUser)
    {
        _questionRepository = questionRepository;
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    public ActionResult Questions(int? page, QuestionsModel model)
    {
        _sessionUiData.QuestionSearchSpec.PageSize = 10;

        _sessionUiData.QuestionSearchSpec.SetFilterByMe(model.FilterByMe);
        _sessionUiData.QuestionSearchSpec.SetFilterByAll(model.FilterByAll);
        _sessionUiData.QuestionSearchSpec.AddFilterByUser(model.AddFilterUser);
        _sessionUiData.QuestionSearchSpec.DelFilterByUser(model.DelFilterUser);
        
        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View("Questions",
                    new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec), _sessionUiData.QuestionSearchSpec, _sessionUser.User.Id)
                    {Pager = new PagerModel(_sessionUiData.QuestionSearchSpec),
                     FilterByMe = _sessionUiData.QuestionSearchSpec.FilterByMe,
                     FilterByAll = _sessionUiData.QuestionSearchSpec.FilterByAll,
                     FilterByUsers =  _userRepository.GetByIds(_sessionUiData.QuestionSearchSpec.FilterByUsers.ToArray())
                                        .ToDictionary(user => user.Id, user => user.Name)});
    }

    [HttpPost]
    public JsonResult DeleteDetails(int questionId)
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

    [HttpPost] 
    public EmptyResult Delete(int questionId)
    {
        Sl.Resolve<QuestionDeleter>().Run(questionId);
        return new EmptyResult();
    }

}
