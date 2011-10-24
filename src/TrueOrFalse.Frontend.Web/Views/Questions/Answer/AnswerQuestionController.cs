using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;

[HandleError]
public class AnswerQuestionController : Controller
{
    private readonly QuestionRepository _questionRepository;

    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public ActionResult Answer(string text, int id)
    {
        var question = _questionRepository.GetById(id);

        return View(_viewLocation, new AnswerQuestionModel(question));
    }

    public JsonResult Answer()
    {
        return null;
    }

}

