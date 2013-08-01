using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class SetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Detail/Set.aspx";

    [SetMenu(MenuEntry.QuestionSetDetail)]
    public ActionResult QuestionSet(string text, int id, int elementOnPage)
    {
        var set = Resolve<QuestionSetRepository>().GetById(id);
        _sessionUiData.VisitedQuestionSets.Add(new QuestionSetHistoryItem(set));

        return View(_viewLocation, new SetModel(set));
    }
}

