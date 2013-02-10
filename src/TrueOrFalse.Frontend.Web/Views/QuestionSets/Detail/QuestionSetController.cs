using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class QuestionSetController : BaseController
{
    private const string _viewLocation = "~/Views/QuestionSets/Detail/QuestionSet.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id, int elementOnPage)
    {
        var set = Resolve<QuestionSetRepository>().GetById(id);
        _sessionUiData.LastQuestionSets.Add(new QuestionSetHistoryItem(set));

        return View(_viewLocation, new QuestionSetModel(set));
    }
}

