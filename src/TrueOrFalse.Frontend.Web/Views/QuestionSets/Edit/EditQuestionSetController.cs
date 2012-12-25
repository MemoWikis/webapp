using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class EditQuestionSetController : Controller
{
    private const string _viewLocation = "~/Views/QuestionSets/Edit/EditQuestionSet.aspx";

    public ActionResult Create()
    {
        return View(_viewLocation, new EditQuestionSetModel());
    }

    public ActionResult Update()
    {
        return View(_viewLocation, new EditQuestionSetModel());
    }
}