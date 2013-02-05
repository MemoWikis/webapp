using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class QuestionSetController : BaseController
{
    private const string _viewLocation = "~/Views/QuestionSets/Detail/QuestionSet.aspx";

    public ActionResult QuestionSet(string text, int id, int elementOnPage)
    {
        var set = Resolve<QuestionSetRepository>().GetById(id);
        return View(_viewLocation, new QuestionSetModel(set));
    }
}

