using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;


public class QuestionSetsController : BaseController
{
    private readonly QuestionSetRepository _setRepo;

    public QuestionSetsController(QuestionSetRepository setRepo)
    {
        _setRepo = setRepo;
    }

    [SetMenu(MenuEntry.QuestionSet)]
    public ActionResult QuestionSets(int? page)
    {
        _sessionUiData.SetSearchSpec.PageSize = 10;
        if (page.HasValue) _sessionUiData.SetSearchSpec.CurrentPage = page.Value;

        var questionSets = _setRepo.GetBy(_sessionUiData.SetSearchSpec);

        return View(new QuestionSetsModel(questionSets, _sessionUser));
    }       
}