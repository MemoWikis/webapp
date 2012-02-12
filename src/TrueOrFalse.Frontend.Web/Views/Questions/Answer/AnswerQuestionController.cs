using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

[HandleError]
public class AnswerQuestionController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly AnswerQuestion _answerQuestion;
    private readonly SessionUser _sessionUser;

    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository, 
                                    AnswerQuestion answerQuestion,
                                    SessionUser sessionUser)
    {
        _questionRepository = questionRepository;
        _answerQuestion = answerQuestion;
        _sessionUser = sessionUser;
    }

    public ActionResult Answer(string text, int id)
    {
        var question = _questionRepository.GetById(id);

        return View(_viewLocation, new AnswerQuestionModel(question));
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer)
    {
        var result = _answerQuestion.Run(id, answer, _sessionUser.User.Id);

        return new JsonResult {Data = new
                                          {
                                              correct = result.IsCorrect, 
                                              correctAnswer = result.CorrectAnswer
                                          }};
    }

    [HttpPost]
    public JsonResult GetAnswer(int id, string answer)
    {
        var question = _questionRepository.GetById(id);
        return new JsonResult { Data = new { correctAnswer = question.Solution} };        
    }

}

