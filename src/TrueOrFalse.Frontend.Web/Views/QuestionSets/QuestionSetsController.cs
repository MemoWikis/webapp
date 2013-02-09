using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;


public class QuestionSetsController : BaseController
{
    private readonly QuestionSetRepository _questionSetRepo;

    public QuestionSetsController(QuestionSetRepository questionSetRepo)
    {
        _questionSetRepo = questionSetRepo;
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult QuestionSets()
    {
        var questionSets = _questionSetRepo.GetBy(_sessionUiData.QuestionSetSearchSpec);

        return View(new QuestionSetsModel(questionSets, _sessionUser));
    }       
}